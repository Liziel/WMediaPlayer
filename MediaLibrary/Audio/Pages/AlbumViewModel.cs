using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.Audio.Pages
{
    public class AlbumViewModel : INotifyPropertyChanged
    {
        private readonly CollectionViewSource _tracksCollectionViewSource = new CollectionViewSource();
        public ListCollectionView TracksView => _tracksCollectionViewSource.View as ListCollectionView;

        public AlbumViewModel()
        {
            Album = MediaPropertiesLibrary.Audio.Library.Library.Albums.FirstOrDefault();
            _tracksCollectionViewSource.Source = Album?.Artists;
        }

        public AlbumViewModel(Album album)
        {
            Album = album;
            _tracksCollectionViewSource.Source = new ObservableCollection<Track>(album.Tracks);
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
    }
}