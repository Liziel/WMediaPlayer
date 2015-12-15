using System;
using System.Globalization;
using System.Windows.Data;

namespace UiLibrary
{
    public class WidthToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as double?) == null)
                return value;
            double conv = (value as double?).Value;

            if (conv > 800)
                return 6;
            else if (conv < 500)
                return 3;
            else
                return 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(value as double?) ?? value;
        }
    }
}