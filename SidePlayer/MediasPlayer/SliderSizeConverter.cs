using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SidePlayer.MediasPlayer
{
    public class SliderSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double))
                return Visibility.Collapsed;
            return (double) values[0] < (double) values[1] == (bool) values[2] ? Visibility.Hidden : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}