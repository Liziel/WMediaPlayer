using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Pictures;
using WPFUiLibrary.UserControls.MediaTemplates.Models;

namespace WPFUiLibrary.UserControls.MediaTemplates.ModelGenerator
{
    public class PicturesFolderModelGenerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PicturesFolderViewModel model = new PicturesFolderViewModel();
            foreach (var value in values)
            {
                if (value is Folder)
                    model.Folder = value as Folder;
            }
            return model;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}