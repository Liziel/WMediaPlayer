using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WPFUiLibrary.UserControls.ContextMenu.BaseItems;
using WPFUiLibrary.UserControls.PopupManager;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class ItemConfigurator
    {
        private static void FillInformations(IEnumerable<Item> items, MenuModel menu)
        {
            foreach (var item in items)
            {
                item.Menu = menu;
                var loaderItem = item as ContentLoaderItem;
                var commandItem = item as CommandItem;
                var userControlItem = item as UserControlItem;
                if (loaderItem != null)
                {
                    loaderItem.Command = new UiCommand(delegate
                    {
                        var loadedItems = loaderItem.Items();
                        FillInformations(loadedItems, menu);
                        menu.PushItems(loadedItems, loaderItem.Name);
                    });
                }
                if (commandItem != null)
                {
                    commandItem.Command = new UiCommand(o =>
                    {
                        commandItem.Action();
                        Dispatch("Remove PopUps", new Func<PopUp, bool>(popUp => ReferenceEquals(popUp.PopUpElement, o)));
                        menu.Reset();
                    });
                }
            }
        }

        public static IEnumerable<Item> Generate(IEnumerable<Item> addItems, MenuModel menu, bool generic)
        {
            var items = (!generic ? new List<Item>() : new List<Item>
            {
                new CommandItem
                {
                    Name = "Play",
                    Action = delegate
                    {
                        Dispatch("Multiple Track Selected For Play", menu.Tracks, 0);
                    }
                },
                new CommandItem
                {
                    Name = "Add to Play Queue",
                    Action = delegate {
                        Dispatch("Multiple Track Selected", menu.Tracks);
                    }
                },
                new ContentLoaderItem
                {
                    Name = "Add to ...",
                    Items =
                    delegate
                    {
                        return new List<Item>
                        {
                            new ContentLoaderItem
                            {
                                Name = "Create Playlist",
                                Items = delegate
                                {
                                    return new List<Item>
                                    {
                                        new UserControlItem
                                        {
                                            UserControl =
                                                new CreatePlaylist(
                                                    name => Dispatch("Playlist: Create Playlist", name, menu.Tracks))
                                        }
                                    };
                                }
                            }
                        }                        
                        .Concat(
                            Request("Playlist: Access Saved Playlists Names")
                                .OfType<IEnumerable<string>>()
                                .SelectMany(enumerable => enumerable)
                                .Select(
                                    name => new CommandItem
                                    {
                                        Name = name,
                                        Action = delegate { Dispatch("Playlist: Add to Saved Playlist", name, menu.Tracks); }
                                    }
                                )).ToList();
                    }
                }
            }).Concat(addItems).ToList();

            FillInformations(items, menu);

            return items;
        }
    }
}