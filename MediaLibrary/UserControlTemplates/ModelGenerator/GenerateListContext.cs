using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using MediaLibrary.UserControlTemplates.Models;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.UserControls.ContextMenu;

namespace MediaLibrary.UserControlTemplates.ModelGenerator
{
    public class GenerateListContext : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ListModel model = new ListModel();
            foreach (var value in values)
            {
                if (value is ListCollectionView)
                    model.List = value as ListCollectionView;
                else if (value is PlayVideoTrack)
                    model.PlayVideoTrack = value as PlayVideoTrack;
                else if (value is PlayAudioTrack)
                    model.PlayAudioTrack = value as PlayAudioTrack;
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