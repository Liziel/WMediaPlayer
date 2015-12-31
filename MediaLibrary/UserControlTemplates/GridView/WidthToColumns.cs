using System;
using System.Globalization;
using System.Windows.Data;

namespace MediaLibrary.UserControlTemplates.GridView
{
    public class WidthToColumns : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as double?) == null)
                return 6;
            double conv = (value as double?).Value;

            if (conv > 1100)
                return 6;
            else if (conv < 500)
                return 3;
            else
                return 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}