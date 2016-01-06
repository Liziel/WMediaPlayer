using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using DispatcherLibrary;
using MediaPropertiesLibrary;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.Models;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.UserControls.ContextMenu.MenuItems;
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

        public SavedPlaylistsViewModel()
        {
            using (
                var file = new FileStream(Locations.Libraries + "/Playlist.xml",
                    FileMode.OpenOrCreate))
                SavedPlaylists =
                    (ObservableCollection<Playlist>)
                        new XmlSerializer(typeof (ObservableCollection<Playlist>)).Deserialize(file);
        }

        public void Save()
        {
            using (
                var file = new FileStream(MediaPropertiesLibrary.Locations.Libraries + "/Playlist.xml",
                    FileMode.Truncate))
                new XmlSerializer(typeof(ObservableCollection<Playlist>)).Serialize(file, SavedPlaylists);
        }

        #region Saved Playlists Management

        public ObservableCollection<Playlist> SavedPlaylists { get; set; } = new ObservableCollection<Playlist>();

        [EventHook("Playlist: Create Playlist")]
        public void CreatePlaylist(string name, IEnumerable<TrackDefinition> tracks)
        {
            var playlist = SavedPlaylists.FirstOrDefault(p => p.Name == name);
            if (playlist != null)
            {
                AddToPlaylist(name, tracks);
                return;
            }
            var newPlaylist = new Playlist {Name = name};
            foreach (var track in tracks)
                newPlaylist.AddTrack(track);
            SavedPlaylists.Add(newPlaylist);
            Save();
        }

        [EventHook("Playlist: Add to Saved Playlist")]
        public void AddToPlaylist(string name, IEnumerable<TrackDefinition> tracks)
        {
            var playlist = SavedPlaylists.FirstOrDefault(p => p.Name == name);
            if (playlist != null)
                foreach (var track in tracks)
                    playlist.AddTrack(track);
            Save();
        }

        [EventHook("Playlist Plugin: Delete Playlist")]
        public void OnPlaylistDelete(Playlist p)
        {
            SavedPlaylists.Remove(p);
            Save();
        }

        [RequestHook("Playlist: Access Saved Playlists Names")]
        public IEnumerable<string> SendPlaylistNames()
        {
            return SavedPlaylists.Select(playlist => playlist.Name);
        }

        #endregion

        #region Play Queue Management

        [EventHook("Playlist Plugin: View In Queue")]
        public void OnQueueView()
        {
            PlayQueue = true;
        }

        public UiCommand ViewInQueue { get; } = new UiCommand(o => Dispatcher.Dispatch("Playlist Plugin: View In Queue"))
            ;

        private bool _playQueue;

        public bool PlayQueue
        {
            get { return _playQueue; }
            set
            {
                _playQueue = value;
                OnPropertyChanged(nameof(PlayQueue));
            }
        }

        #endregion

        public UiCommand ViewPlaylist { get; } = new UiCommand(o => Dispatcher.Dispatch("Playlist Plugin: View Playlist", o));

        [EventHook("Playlist Plugin: View Playlist")]
        public void OnPlaylistView(Playlist p)
        {
            CurrentPlaylist = p;
            PlayQueue = false;
        }

        private Playlist _currentPlaylist;
        public Playlist CurrentPlaylist
        {
            get { return _currentPlaylist; }
            set { _currentPlaylist = value; OnPropertyChanged(nameof(CurrentPlaylist));}
        }
    }
}