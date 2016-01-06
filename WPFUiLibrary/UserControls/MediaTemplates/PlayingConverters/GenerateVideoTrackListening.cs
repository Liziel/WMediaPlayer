using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DispatcherLibrary;
using MediaPropertiesLibrary.Video;
using MediaPropertiesLibrary.Video.Library;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.PlayingConverters
{
    public class GenerateVideoTrackListening : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VideoTrackViewModel model = value as VideoTrackViewModel;
            if (model?.Track == null) return null;
            if (model.PlayVideoTrack != null) return new UiCommand(o => model.PlayVideoTrack(model.Track));
            if (model.Track.Serie != null)
                return
                    new UiCommand(
                        o =>
                            DispatcherLibrary.Dispatcher.Dispatch("Multiple Track Selected For Play", model.Track.Serie.Tracks,
                                model.Track.Serie.Tracks.FindIndex(t => t == model.Track)));
            return new UiCommand(o => Dispatcher.Dispatch("Multiple Track Selected For Play", new List<Track> { model.Track }, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}