using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DispatcherLibrary;
using MediaLibrary.UserControlTemplates.Models;
using MediaLibrary.UserControlTemplates.PlayingConverters;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.Utils;

namespace MediaLibrary.UserControlTemplates.ModelGenerator
{
    public class GenerateMenuModel : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = new MenuModel();
            var album = value as Album;
            var track = value as TrackDefinition;
            var artist = value as Artist;
            if (album != null)
                model.Tracks = album.Tracks;
            else if (track != null)
                model.Tracks = new List<TrackDefinition> { track };
            else if (artist != null)
                model.Tracks = MediaPropertiesLibrary.Audio.Library.Library.Tracks.Where(t => t.Artists.Contains(artist));
            return model;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}