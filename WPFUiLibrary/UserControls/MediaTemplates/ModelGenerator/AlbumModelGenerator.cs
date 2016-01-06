using System;
using System.Globalization;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.UserControls.MediaTemplates.Models;

namespace WPFUiLibrary.UserControls.MediaTemplates.ModelGenerator
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