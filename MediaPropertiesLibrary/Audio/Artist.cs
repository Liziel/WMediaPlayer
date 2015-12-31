using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaPropertiesLibrary.Annotations;

namespace MediaPropertiesLibrary.Audio
{
    public class Artist : INotifyPropertyChanged
    {
        private MediaState _state = MediaState.Stopped;

        public string Name { get; set; }
        public ArtistUserTag Tag = new ArtistUserTag();

        [XmlIgnore]
        public HashSet<Album> Albums { get; } = new HashSet<Album>();
        [XmlIgnore]
        public List<Track> Singles { get; } = new List<Track>();
        [XmlIgnore]
        public MediaState State { get { return _state; } set { _state = value; OnPropertyChanged(nameof(State)); } }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class ArtistUserTag
    {
        public string Picture;
    }

    public class AccessArtistCover : IMultiValueConverter
    {
        public static ImageSource AccessCover(Artist artist)
        {
            if (!string.IsNullOrEmpty(artist.Tag?.Picture) && File.Exists(artist.Tag.Picture))
                return new BitmapImage(new Uri(artist.Tag.Picture));
            return artist.Albums.FirstOrDefault(album => album.Cover != null)?.Cover;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is Artist)) return null;
            var artist = (Artist)values[0];

            return AccessCover(artist);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}