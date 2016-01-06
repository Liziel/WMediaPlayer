using System.ComponentModel;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.ChildsViews;
using PlaylistPlugin.Models;
using static DispatcherLibrary.Dispatcher;

namespace PlaylistPlugin
{

    public sealed class PlaylistMainViewModel : Listener, INotifyPropertyChanged
    {
        #region Childs Views

        private object _playlistView;
        public object PlaylistView
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
            Dispatch("Playlist Plugin: View In Queue");
        }

        [EventHook("Playlist Plugin: View In Queue")]
        public void ViewInQueue()
        {
            PlaylistView = CurrentPlaylist;
        }

        [EventHook("Playlist Plugin: View Playlist")]
        public void OnPlaylistView(Playlist p)
        {
            PlaylistView = new PlaylistView {DataContext = new PlaylistViewModel {Playlist = p} };
        }

    }
}