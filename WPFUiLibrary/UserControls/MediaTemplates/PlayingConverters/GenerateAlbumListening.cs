using System;
using System.Globalization;
using System.Windows.Data;
using DispatcherLibrary;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.PlayingConverters
{
    public class GenerateAlbumListening : IValueConverter
    {
        public static UiCommand Generate(AudioAlbumViewModel model)
        {
            if (model == null || model.Album == null) return null;
            if (model.PlayAlbum != null) return new UiCommand(o => model.PlayAlbum(model.Album));
            return new UiCommand(o => Dispatcher.Dispatch("Multiple Track Selected For Play", model.Album.Tracks, 0));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Generate(value as AudioAlbumViewModel);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}