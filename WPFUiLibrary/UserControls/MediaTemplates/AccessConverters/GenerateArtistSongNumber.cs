using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Audio.Library;

namespace WPFUiLibrary.UserControls.MediaTemplates.AccessConverters
{
    public class GenerateArtistSongNumber : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Artist artist = value as Artist;
            if (artist == null) return 0;
            return
                Library
                .Songs
                .Count(track => track.Artists.Contains(artist));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}