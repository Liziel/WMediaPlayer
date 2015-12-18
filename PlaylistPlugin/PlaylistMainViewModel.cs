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

namespace PlaylistPlugin
{

    [Export(typeof(IMessageablePlugin))]
    public sealed class PlaylistMainViewModel : Listener, IMessageablePlugin, INotifyPropertyChanged
    {
        #region Childs Views

        private SavedPlaylistsViewModel _savedPlaylists = new SavedPlaylistsViewModel();
        private CurrentPlaylistViewModel _currentPlaylist = new CurrentPlaylistViewModel();

        [ForwardDispatch]
        public SavedPlaylistsViewModel SavedPlaylists => _savedPlaylists;
        [ForwardDispatch]
        public CurrentPlaylistViewModel CurrentPlaylist => _currentPlaylist;

        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IMessageablePlugin Properties

        public event MessageableStatusChanged StatusChanged;
        public bool Optional { get; } = false;

        private void OnMessageableStatusChanged(object source)
        {
            StatusChanged?.Invoke(source);
        }

        #endregion
    }
}