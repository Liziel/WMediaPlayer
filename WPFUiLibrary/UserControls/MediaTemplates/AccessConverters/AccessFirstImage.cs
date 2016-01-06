using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using MediaPropertiesLibrary.Pictures;
using WPFUiLibrary.Resources;

namespace WPFUiLibrary.UserControls.MediaTemplates.AccessConverters
{

    public class AccessFirstImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Folder folder = value as Folder;
            if (folder == null) return null;
            return folder.Pictures.FirstOrDefault()?.Path ??
                   folder.Folders.SelectRecursive(f => f.Folders).SelectMany(f => f.Pictures).FirstOrDefault()?.Path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}