using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.AccessConverters
{
    public class GenerateAccessArtistFromTrack : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Track track = value as Track;
            if (track == null) return null;
            if (track.Artists.Count == 0) return null;
            return new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("AudioLibrary: View Artist", track.Artists.FirstOrDefault()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}