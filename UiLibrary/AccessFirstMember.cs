using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace UiLibrary
{
    
    public class AccessFirstMember
        : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var l = value as List<object>;
            if (l != null)
                return l.Find(o => true);
            var l2 = value as HashSet<object>;
            if (l2 != null)
                return l2;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(value as double?) ?? value;
        }
    }
}