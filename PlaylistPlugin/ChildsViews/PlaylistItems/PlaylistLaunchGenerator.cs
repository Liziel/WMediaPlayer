using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using PlaylistPlugin.Models;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using static DispatcherLibrary.Dispatcher;

namespace PlaylistPlugin.ChildsViews.PlaylistItems
{
    public class PlaylistAudioLaunchGenerator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playlist = value as Playlist;
            return playlist == null ? null : new PlayAudioTrack(track => Dispatch("Multiple Track Selected For Play", playlist.Tracks.Select(member => member.Track), playlist.Tracks.FindIndex(member => member.Track == track)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PlaylistVideoLaunchGenerator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var playlist = value as Playlist;
            return playlist == null ? null : new PlayVideoTrack(track => Dispatch("Multiple Track Selected For Play", playlist.Tracks.Select(member => member.Track), playlist.Tracks.FindIndex(member => member.Track == track)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}