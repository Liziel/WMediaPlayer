using System;
using System.Globalization;
using System.Windows.Data;
using MediaLibrary.UserControlTemplates.Models;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.UserControlTemplates.ModelGenerator
{
    public class TrackModelGenerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            AudioTrackViewModel model = new AudioTrackViewModel();
            foreach (var value in values)
            {
                if (value is Track)
                    model.Track = value as Track;
                else if (value is PlayAudioTrack)
                    model.PlayAudioTrack = value as PlayAudioTrack;
            }
            return model;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}