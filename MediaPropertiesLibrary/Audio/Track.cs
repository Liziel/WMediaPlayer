using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Audio
{
    public class Track : MediaPropertiesLibrary.TrackDefinition
    {
        #region Serializable Members

        public string Name { get; set; }
        public string Path { get; set; }
        public TrackUserTag UserTag { get; set; }
        public List<string> Genres { get; set; }

        [XmlElement("Duration")]
        public long TimeSpanDuration { get { return Duration.Ticks; } set { Duration = new TimeSpan(value); } }

        #endregion

        #region Attached Members

        [XmlIgnore]
        public override TimeSpan Duration { get; set; }
        [XmlIgnore]
        public Album Album { get; set; }
        [XmlIgnore]
        public List<Artist> Artists { get; set; } = new List<Artist>();
        [XmlIgnore]
        public List<string> RelativePaths { get; set; }

        #endregion

        #region Herited From ITracks

        public override string MediaLibraryKey => Path;

        protected override void OnStateChanged(MediaState state)
        {
            if (state == MediaState.End) return;
            if (Album != null)
                Album.State = State;
            foreach (var artist in Artists)
                artist.State = state;
        }

        #endregion

    }

    public sealed class TrackUserTag
    {
        public int TimesListened { get; set; } = 0;
        public bool Favored { get; set; } = false;
    }

    public class TrackAccessCover : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;
            return track?.Album?.Cover;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TrackAccessArtistName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;
            if (track == null)
                return "";
            return track.Artists?.Count > 0 ? track.Artists[0].Name : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TrackAccessAlbumName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Track)?.Album?.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TrackDurationStylized : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var track = value as Track;
            if (track == null)
                return "";
            return track.Duration > TimeSpan.FromHours(1) ? track.Duration.ToString(@"hh\:mm\:ss") : track.Duration.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}