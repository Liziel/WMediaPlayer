using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using PlaylistPlugin.Models;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.UserControls.ContextMenu.MenuItems;

namespace PlaylistPlugin.ChildsViews.PlaylistItems
{
    public class PlaylistMenuGenerator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playlist = value as Playlist;
            if (playlist == null) return null;
            return new MenuModel(
                new List<Item>
                {
                    new CommandItem
                    {
                        Name = "Delete",
                        Action = delegate { DispatcherLibrary.Dispatcher.Dispatch("Playlist Plugin: Delete Playlist", playlist); }
                    }
                }
            )
            {
                Tracks = playlist.Tracks.Select(member => member.Track).ToList()
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}