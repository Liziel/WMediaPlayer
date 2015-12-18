using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.Audio.SubViews
{
    public class AudioAlbumViewModel : Listener, INotifyPropertyChanged
    {
        private List<Album> _albumsAccess = null;
        public List<Album> AlbumsAccess => _albumsAccess;

        private readonly CollectionViewSource _albumCollectionViewSource = new CollectionViewSource();
        public ICollectionView AlbumsView => _albumCollectionViewSource.View;
        public int? AlbumsViewCount => AlbumsView?.Cast<Album>().Count();

        private string _searchText = "";

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                AlbumsView?.Refresh();
                OnPropertyChanged(nameof(AlbumsViewCount));
            }
        }

        public AudioAlbumViewModel()
        {
            MediaPropertiesLibrary.Audio.Library.Library.TracksLoaded += () =>
            {
               _albumsAccess = MediaPropertiesLibrary.Audio.Library.Library.Albums;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action(delegate
                        {
                            _albumCollectionViewSource.Source = _albumsAccess;
                            AlbumsView.Filter += item => ((Album) item).Name.ToLower().Contains(SearchText.ToLower());
                            AlbumsView.Refresh();
                            OnPropertyChanged(nameof(AlbumsView));
                            OnPropertyChanged(nameof(AlbumsViewCount));
                        }), DispatcherPriority.DataBind);
            };
        }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}