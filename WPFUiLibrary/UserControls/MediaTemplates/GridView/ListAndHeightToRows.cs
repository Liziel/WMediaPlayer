using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFUiLibrary.UserControls.MediaTemplates.GridView
{
    public class ListAndHeightToRows : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ListCollectionView value = values[0] as ListCollectionView;
            double width = (double)values[1];
            if (value == null) return null;
            if (width > 1100)
                return value.Count / 6 + (value.Count % 6 > 0 ? 1 : 0);
            if (width > 500)
                return value.Count/4 + (value.Count % 4 > 0 ? 1 : 0);
            return value.Count/3 + (value.Count % 3 > 0 ? 1 : 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}