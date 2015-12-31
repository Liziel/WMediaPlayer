using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;

namespace MediaLibrary.UserControlTemplates.AccessConverters
{
    public class GenerateAccessAlbumFromTrack : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Track track = value as Track;
            if (track?.Album == null) return null;
            return new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("AudioLibrary: View Album", track.Album));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}