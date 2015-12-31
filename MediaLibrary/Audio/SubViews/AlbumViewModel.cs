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
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;
using Dispatcher = DispatcherLibrary.Dispatcher;

namespace MediaLibrary.Audio.SubViews
{
    public class AlbumViewModel : Listener, INotifyPropertyChanged
    {
        private readonly CollectionViewSource _albumCollectionViewSource = new CollectionViewSource();
        public ListCollectionView AlbumsView => _albumCollectionViewSource.View as ListCollectionView;
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

        public AlbumViewModel()
        {
            _albumCollectionViewSource.Source = MediaPropertiesLibrary.Audio.Library.Library.Albums;
            AlbumsView.Filter += item => ((Album)item).Name.ToLower().Contains(SearchText.ToLower());
            AlbumsView.CustomSort =
                Comparer<Album>.Create(
                    (album1, album2) =>
                        string.Compare(album1.Name, album2.Name, StringComparison.CurrentCultureIgnoreCase));
            AlbumsView.Refresh();
            OnPropertyChanged(nameof(AlbumsView));
            OnPropertyChanged(nameof(AlbumsViewCount));
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