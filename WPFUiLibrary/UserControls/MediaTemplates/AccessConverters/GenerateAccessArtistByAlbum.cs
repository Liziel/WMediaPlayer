using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DispatcherLibrary;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.AccessConverters
{
    public class GenerateAccessArtistByAlbum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var album = value as Album;
            if (album == null) return null;
            if (album.Artists.Count == 0) return null;
            return new UiCommand(o => Dispatcher.Dispatch("AudioLibrary: View Artist", album.Artists.FirstOrDefault()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}