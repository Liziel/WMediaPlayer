using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFUiLibrary.Converters
{
    public class CountToRow : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as int?) == null)
                return false;
            int conv = ((int?) value).Value;

            if (conv > 9)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(value as double?) ?? value;
        }
    }
}