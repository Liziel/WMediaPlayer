using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MediaLibrary.UserControlTemplates.Models;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;
using static MediaPropertiesLibrary.Audio.Library.Library;

namespace MediaLibrary.UserControlTemplates.PlayingConverters
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
                        Dispatch(
                            "Multiple Track Selected For Play",
                            Tracks.Where(track => track.Artists.Contains(model.Artist)),
                            0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}