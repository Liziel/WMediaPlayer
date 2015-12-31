using System;
using System.Globalization;
using System.Windows.Data;

namespace MediaLibrary.UserControlTemplates.GridView
{
    public class ListAndHeightToRows : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ListCollectionView value = values[0] as ListCollectionView;
            if (value == null) return null;
            if (value.Count > 9) return null;
            return 3;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}