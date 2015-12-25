using System;
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
using UiLibrary.Utils;

namespace PlaylistPlugin.ChildsViews
{
    internal enum Loop
    {
        None, One, All
    }

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
        public TrackDefinition CurrentMedia => Current?.Tracks[PlayOffset].Track;
        public long CurrentMediaPosition => Current.Tracks[PlayOffset].Position;
        private int PlayOffset
        {
            get { return _playOffset; }
            set
            {
                if (CurrentMedia != null)
                    CurrentMedia.PropertyChanged -= CurrentMedia_PropertyChanged;
                _playOffset = value;
                if (CurrentMedia != null)
                    CurrentMedia.PropertyChanged += CurrentMedia_PropertyChanged;
                OnPropertyChanged(nameof(PlayOffset));
                OnPropertyChanged(nameof(CurrentMedia));
                OnPropertyChanged(nameof(CurrentMediaPosition));
            }
        }

        private void CurrentMedia_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CurrentMedia.State == MediaState.End)
                NextTrack();
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
        public void AddToPlaylist(TrackDefinition trackDefinition)
        {
            if (Current == null)
            {
                MultipleAddToPlaylistAndPlay(new[] {trackDefinition}, 0);
                return;
            }
            Current.AddTrack(trackDefinition);
            _currentTracks.Source = Current.Tracks;
            CurrentTracks.Refresh();
            OnPropertyChanged(nameof(CurrentTracks));
        }

        [EventHook("Multiple Track Selected")]
        public void MultipleAddToPlaylist(IEnumerable<TrackDefinition> tracks)
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
        public void MultipleAddToPlaylistAndPlay(IEnumerable<TrackDefinition> tracks, int index)
        {
            Current = new Playlist();
            var off = Current.Tracks.Count + index;
            foreach (var track1 in tracks)
                Current.AddTrack(track1);
            PlayOffset = off;
            Dispatcher.GetInstance.Dispatch("Play", CurrentMedia);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (Current.Tracks.Count > 50)
                _currentTracks.Source = Current.Tracks.Skip(PlayOffset).Take(50);
            else if (Current.Tracks.Count - PlayOffset < 50)
                _currentTracks.Source = Current.Tracks.Skip(Math.Max(0, Current.Tracks.Count - 50));
            else
                _currentTracks.Source = Current.Tracks;
            CurrentTracks.Refresh();
            OnPropertyChanged(nameof(CurrentTracks));
        }

        #endregion

        #region Tracks Play Control

        private Loop _loop = Loop.None;

        [EventHook("Previous Track")]
        public void PreviousTrack()
        {
            if (_loop != Loop.One)
            {
                if (PlayOffset - 1 >= 0)
                    PlayOffset -= 1;
                else
                    switch (_loop)
                    {
                        case Loop.All:
                            PlayOffset = Current.Tracks.Count - 1;
                            break;
                        case Loop.None:
                            break;
                    }
            }
            Dispatcher.GetInstance.Dispatch("Play", CurrentMedia);
            RefreshDisplay();
        }

        [EventHook("Next Track")]
        public void NextTrack()
        {
            if (_loop != Loop.One)
            {
                if (PlayOffset + 1 < Current.Tracks.Count)
                    PlayOffset += 1;
                else
                    switch (_loop)
                    {
                        case Loop.All:
                            PlayOffset = 0;
                            break;
                        case Loop.None:
                            return;
                    }
            }
            Dispatcher.GetInstance.Dispatch("Play", CurrentMedia);
            RefreshDisplay();
        }

        #endregion

        public UiCommand GoToVideos { get; } = new UiCommand(o => Dispatcher.GetInstance.Dispatch("Loader: Call(My Videos)"));
        public UiCommand GoToMusics { get; } = new UiCommand(o => Dispatcher.GetInstance.Dispatch("Loader: Call(My Musics)"));
    }
}