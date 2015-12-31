using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using MediaLibrary.UserControlTemplates.Models;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.UserControlTemplates.ModelGenerator
{
    public class AlbumModelGenerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            AudioAlbumViewModel model = new AudioAlbumViewModel();
            foreach (var value in values)
            {
                if (value is Album)
                    model.Album = value as Album;
                else if (value is PlayAlbum)
                    model.PlayAlbum = value as PlayAlbum;
            }
            return model;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}