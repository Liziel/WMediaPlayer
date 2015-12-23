using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary;
using PlaylistPlugin.Models;
using UiLibrary;

namespace PlaylistPlugin.ChildsViews
{
    public class CurrentPlaylistViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Playlist Management

        private int _playOffset = 0;
        public ITrack CurrentMedia => Current.Tracks[PlayOffset].Track;
        public long CurrentMediaPosition => Current.Tracks[PlayOffset].Position;
        private int PlayOffset
        {
            get { return _playOffset; }
            set
            {
                _playOffset = value;
                OnPropertyChanged(nameof(PlayOffset));
                OnPropertyChanged(nameof(CurrentMedia));
                OnPropertyChanged(nameof(CurrentMediaPosition));
            }
        }

        private Playlist _current;
        public Playlist Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                OnPropertyChanged(nameof(Current));
            }
        }


        private readonly CollectionViewSource _currentTracks = new CollectionViewSource();
        public ICollectionView CurrentTracks => _currentTracks.View;

        [EventHook("Set Current Playlist")]
        public void SetCurrentPlaylist(Playlist playlist)
        {
            Current = playlist;
        }

        [EventHook("Track Selected")]
        public void AddToPlaylist(ITrack track)
        {
            if (Current == null)
            {
                MultipleAddToPlaylistAndPlay(new[] {track}, 0);
                return;
            }
            Current.AddTrack(track);
            _currentTracks.Source = Current.Tracks;
            CurrentTracks.Refresh();
            OnPropertyChanged(nameof(CurrentTracks));
        }

        [EventHook("Multiple Track Selected")]
        public void MultipleAddToPlaylist(IEnumerable<ITrack> tracks)
        {
            if (Current == null)
            {
                MultipleAddToPlaylistAndPlay(tracks, 0);
                return;
            }
            foreach (var track in tracks)
                Current.AddTrack(track);
            _currentTracks.Source = Current.Tracks;
            CurrentTracks.Refresh();
            OnPropertyChanged(nameof(CurrentTracks));
        }

        [EventHook("Multiple Track Selected For Play")]
        public void MultipleAddToPlaylistAndPlay(IEnumerable<ITrack> tracks, int index)
        {
            Current = new Playlist();
            var off = Current.Tracks.Count + index;
            foreach (var track1 in tracks)
                Current.AddTrack(track1);
            PlayOffset = off;
            _currentTracks.Source = Current.Tracks.Skip(off).Take(50);
            Dispatcher.GetInstance.Dispatch("Play", CurrentMedia);
            CurrentTracks.Refresh();
            OnPropertyChanged(nameof(CurrentTracks));
        }

        #endregion

        public UiCommand GoToVideos { get; } = new UiCommand(o => Dispatcher.GetInstance.Dispatch("Loader: Call(My Videos)"));
        public UiCommand GoToMusics { get; } = new UiCommand(o => Dispatcher.GetInstance.Dispatch("Loader: Call(My Musics)"));

    }

}