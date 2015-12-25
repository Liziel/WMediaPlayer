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
            Cover = album.Cover;
            Name = album.Name;
            Tracks = album.Tracks;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private BitmapImage _cover;
        public BitmapImage Cover
        {
            get { return _cover; }
            set
            {
                _cover = value; 
                OnPropertyChanged(nameof(Cover));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                OnPropertyChanged(nameof(Name));
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