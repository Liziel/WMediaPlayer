using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using MediaPropertiesLibrary;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.ChildsViews.PlaylistItems;
using PlaylistPlugin.Models;

namespace PlaylistPlugin.ChildsViews
{
    public class PlaylistViewModel
    {
        private Playlist _playlist;
        public Playlist Playlist
        {
            get { return _playlist; }
            set { _playlist = value; _collectionView.Source = value.Tracks;}
        }

        private PlaylistHeaderViewModel _headerModel = null;
        public PlaylistHeaderViewModel HeaderModel => _headerModel ?? (_headerModel = new PlaylistHeaderViewModel(Playlist));

        private readonly CollectionViewSource _collectionView = new CollectionViewSource();
        public ListCollectionView PlaylistView => _collectionView?.View as ListCollectionView;
    }
}