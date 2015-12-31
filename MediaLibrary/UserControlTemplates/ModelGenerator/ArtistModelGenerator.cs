using System;
using System.Globalization;
using System.Windows.Data;
using MediaLibrary.UserControlTemplates.Models;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.UserControlTemplates.ModelGenerator
{
    public class ArtistModelGenerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            AudioArtistViewModel model = new AudioArtistViewModel();
            foreach (var value in values)
            {
                if (value is Artist)
                    model.Artist = value as Artist;
                else if (value is PlayArtist)
                    model.PlayArtist = value as PlayArtist;
            }
            return model;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}