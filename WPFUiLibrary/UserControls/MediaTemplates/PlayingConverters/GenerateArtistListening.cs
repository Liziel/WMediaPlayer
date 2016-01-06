using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DispatcherLibrary;
using MediaPropertiesLibrary.Audio.Library;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.PlayingConverters
{
    public class GenerateArtistListening : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = value as AudioArtistViewModel;
            if (model == null || model.Artist == null) return null;
            if (model.PlayArtist != null) return new UiCommand(o => model.PlayArtist(model.Artist));
            return
                new UiCommand(
                    o =>
                        Dispatcher.Dispatch(
                            "Multiple Track Selected For Play",
                            Library.Songs.Where(track => track.Artists.Contains(model.Artist)),
                            0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}