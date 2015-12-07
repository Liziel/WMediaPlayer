using System;
using System.Globalization;
using System.Windows.Data;

namespace DefaultMWMP2toolbar
{
    public class NegatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(value as double?) - 30 ?? value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(value as double?) ?? value;
        }
    }
}