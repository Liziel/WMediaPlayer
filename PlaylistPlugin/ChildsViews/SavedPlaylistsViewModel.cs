using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.Models;
using WPFUiLibrary.Utils;

namespace PlaylistPlugin.ChildsViews
{
    public class SavedPlaylistsViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Saved Playlists Management

        private List<Playlist> _savedPlaylists;

        public List<Playlist> SavedPlaylists
        {
            get { return _savedPlaylists; }
            set
            {
                _savedPlaylists = value;
                OnPropertyChanged(nameof(SavedPlaylists));
            }
        }

        [EventHook("Save Playlist")]
        public void OnSavedPlaylist(Playlist playlist)
        {
            SavedPlaylists.Add(playlist);
            OnPropertyChanged(nameof(SavedPlaylists));
        }

        #endregion

        public UiCommand ViewInQueue { get; } = new UiCommand(o => Dispatcher.Dispatch("Playlist Plugin: View In Queue"));
    }
}