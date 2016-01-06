using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Pictures;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.UserControls.ContextMenu.MenuItems;

namespace WPFUiLibrary.UserControls.MediaTemplates.ModelGenerator
{
    public class GenerateMenuModel : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var folder = value as Folder;
            if (folder != null)
                return new MenuModel(new List<Item>
                {
                    new CommandItem
                    {
                        Name = "View in Slideshow",
                        Action = delegate
                        {
                            DispatcherLibrary.Dispatcher.Dispatch("Picture Library: Display Folder", folder);
                        }
                    }
                }, false);
            var album = value as Album;
            var track = value as TrackDefinition;
            var artist = value as Artist;
            var model = new MenuModel();
            if (album != null)
                model.Tracks = album.Tracks;
            else if (track != null)
                model.Tracks = new List<TrackDefinition> { track };
            else if (artist != null)
                model.Tracks = MediaPropertiesLibrary.Audio.Library.Library.Songs.Where(t => t.Artists.Contains(artist));
            return model;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}