using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using MediaPropertiesLibrary.Audio;
using PlaylistPlugin.Models;

namespace PlaylistPlugin.Ressources
{
    public class AccessAlbumCoverFromPlaylist : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var playlist = values[0] as ObservableCollection<Playlist.Member>;
            var offset = (long)values[1];
            return playlist?.Select(member => (member.Track as Track)?.Album?.Cover ?? (member.Track as MediaPropertiesLibrary.Video.Track)?.Picture).Distinct().Where(image => image != null).Skip((int)offset).FirstOrDefault();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}