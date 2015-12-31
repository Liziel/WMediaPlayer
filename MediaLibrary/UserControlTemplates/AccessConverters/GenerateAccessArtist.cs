using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;

namespace MediaLibrary.UserControlTemplates.AccessConverters
{
    public class GenerateAccessArtist : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Artist artist = value as Artist;
            if (artist == null) return null;
            return new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("AudioLibrary: View Artist", artist));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}