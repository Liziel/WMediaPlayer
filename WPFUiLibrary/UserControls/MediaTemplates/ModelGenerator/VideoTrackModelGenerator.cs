using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Video;
using WPFUiLibrary.UserControls.MediaTemplates.Models;

namespace WPFUiLibrary.UserControls.MediaTemplates.ModelGenerator
{
    public class VideoTrackModelGenerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            VideoTrackViewModel model = new VideoTrackViewModel();
            foreach (var value in values)
            {
                if (value is Track)
                    model.Track = value as Track;
                else if (value is PlayVideoTrack)
                    model.PlayVideoTrack = value as PlayVideoTrack;
            }
            return model;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}