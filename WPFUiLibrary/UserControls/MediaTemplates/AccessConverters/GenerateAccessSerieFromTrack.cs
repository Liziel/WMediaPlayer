using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Video;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.AccessConverters
{
    public class GenerateAccessSerieFromTrack : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;
            if (track == null) return null;
            return new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("VideoLibrary: View Serie", track.Serie));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}