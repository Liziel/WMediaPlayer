using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DispatcherLibrary;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Audio.Library;
using WPFUiLibrary.UserControls.MediaTemplates.Models;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.MediaTemplates.PlayingConverters
{
    public class GenerateAudioTrackListening : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AudioTrackViewModel model = value as AudioTrackViewModel;
            if (model?.Track == null) return null;
            if (model.PlayAudioTrack != null) return new UiCommand(o => model.PlayAudioTrack(model.Track));
            if (model.Track.Album != null)
                return
                    new UiCommand(
                        o =>
                            DispatcherLibrary.Dispatcher.Dispatch("Multiple Track Selected For Play", model.Track.Album.Tracks,
                                model.Track.Album.Tracks.FindIndex(t => t == model.Track)));
            if (model.Track.Artists.Count > 0)
            {
                var artist = model.Track.Artists.FirstOrDefault();
                var npl = Library.Songs.Where(t => t.Artists.Contains(artist)).ToList();
                return 
                    new UiCommand(o => Dispatcher.Dispatch("Multiple Track Selected For Play", npl, npl.FindIndex(t => t == model.Track)));
            }
            return new UiCommand(o => Dispatcher.Dispatch("Multiple Track Selected For Play", new List<Track> {model.Track}, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}