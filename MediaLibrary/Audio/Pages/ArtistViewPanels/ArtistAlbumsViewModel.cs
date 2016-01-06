using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.UserControls.MediaTemplates.Models;

namespace MediaLibrary.Audio.Pages.ArtistViewPanels
{
    public sealed class ArtistAlbumsViewModel : INotifyPropertyChanged
    {
        private readonly CollectionViewSource _albums = new CollectionViewSource();
        public ListCollectionView Albums => _albums.View as ListCollectionView;

        public PlayAlbum PlayAlbum { get; }

        public bool GridView
        {
            get { return _gridView; }
            set
            {
                _gridView = value; 
                OnPropertyChanged(nameof(GridView));
            }
        }

        public bool ListView
        {
            get { return _listView; }
            set
            {
                _listView = value; 
                OnPropertyChanged(nameof(ListView));
            }
        }

        private bool _gridView = true;
        private bool _listView;

        public ArtistAlbumsViewModel(Artist artist)
        {
            PlayAlbum = new PlayAlbum(album =>
            {
                var l =
                    MediaPropertiesLibrary.Audio.Library.Library.Songs.Where(track => track.Artists.Contains(artist))
                        .ToList();
                DispatcherLibrary.Dispatcher.Dispatch("Multiple Track Selected For Play", l, l.FindIndex(track => track.Album == album));
            });
            _albums.Source = new ObservableCollection<Album>(artist.Albums);
            Albums.Refresh();
            OnPropertyChanged(nameof(Albums));
        }

        #region Notifier properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}