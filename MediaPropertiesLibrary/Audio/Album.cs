using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaPropertiesLibrary.Annotations;

namespace MediaPropertiesLibrary.Audio
{
    public class Album : INotifyPropertyChanged
    {
        private MediaState _state = MediaState.Stopped;
        private BitmapImage _cover = null;

        public string Name { get; set; }

        [XmlIgnore]
        public BitmapImage Cover
        {
            get { return _cover; }
            set
            {
                _cover = value;
                OnPropertyChanged(nameof(Cover));
                foreach (var artist in Artists)
                    artist.OnPropertyChanged(nameof(artist.Name));
            }
        }
        [XmlIgnore]
        public HashSet<Artist>  Artists { get; set; } = new HashSet<Artist>();
        [XmlIgnore]
        public List<Track> Tracks { get; } = new List<Track>();

        [XmlIgnore]
        public MediaState State { get { return _state; } set { _state = value; OnPropertyChanged(nameof(State)); } }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class AlbumAccessArtistName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var album = value as Album;
            if (album == null)
                return "";
            return album.Artists?.Count > 0 ? album.Artists.First().Name : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}