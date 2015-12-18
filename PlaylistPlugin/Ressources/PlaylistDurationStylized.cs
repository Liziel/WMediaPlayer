using System;
using System.Globalization;
using System.Windows.Data;
using PlaylistPlugin.Models;

namespace PlaylistPlugin.Ressources
{
    public class PlaylistDurationStylized : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Playlist;
            if (track == null)
                return "";
            return track.Duration > TimeSpan.FromHours(1)
                ? track.Duration.ToString(@"hh\:mm\:ss")
                : track.Duration.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}