using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;

namespace MediaLibrary.UserControlTemplates.AccessConverters
{
    public class GenerateAccessAlbum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Album album = value as Album;
            if (album == null) return null;
            return new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("AudioLibrary: View Album", album));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}