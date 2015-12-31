using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.ChildsViews;
using PlaylistPlugin.Models;
using PluginLibrary;
using System.ComponentModel.Composition;
using static DispatcherLibrary.Dispatcher;

namespace PlaylistPlugin
{

    public sealed class PlaylistMainViewModel : Listener, INotifyPropertyChanged
    {
        #region Childs Views

        private Listener _playlistView;
        public Listener PlaylistView
        {
            get { return _playlistView; }
            set
            {
                _playlistView = value;
                OnPropertyChanged(nameof(PlaylistView));
            }
        }

        [ForwardDispatch]
        public SavedPlaylistsViewModel SavedPlaylists { get; } = new SavedPlaylistsViewModel();
        [ForwardDispatch]
        public CurrentPlaylistViewModel CurrentPlaylist { get; } = new CurrentPlaylistViewModel();
        [ForwardDispatch]
        public object PlaylistDisplay { get; } = null;


        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public PlaylistMainViewModel()
        {
            AddEventListener(this);
            ViewInQueue();
        }

        [EventHook("Playlist Plugin: View In Queue")]
        public void ViewInQueue()
        {
            PlaylistView = CurrentPlaylist;
        }

    }
}