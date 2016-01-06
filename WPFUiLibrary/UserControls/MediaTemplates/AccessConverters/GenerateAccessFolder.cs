using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Pictures;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace WPFUiLibrary.UserControls.MediaTemplates.AccessConverters
{
    public class GenerateAccessFolder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Folder folder = value as Folder;
            if (folder == null) return null;
            return new UiCommand(o => Dispatch("PictureLibrary: View Folder", folder));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}