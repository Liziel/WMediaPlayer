using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.Audio.Pages
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        public AlbumViewModel(Album album)
        {
            Album = album;
            Tracks = album.Tracks;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Album _album;

        public Album Album
        {
            get { return _album; }
            set
            {
                _album = value;
                OnPropertyChanged(nameof(Album));
            }
        }

        private List<Track> _tracks;

        public List<Track> Tracks
        {
            get { return _tracks; }
            set
            {
                _tracks = value;
                OnPropertyChanged(nameof(Tracks));
            }
        } 
    }
}