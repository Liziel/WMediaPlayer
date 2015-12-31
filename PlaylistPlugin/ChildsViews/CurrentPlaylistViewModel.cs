using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DispatcherLibrary;
using MediaPropertiesLibrary;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.Models;
using PlaylistPlugin.Ressources;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace PlaylistPlugin.ChildsViews
{
    internal enum Loop
    {
        None, One, All
    }

    public sealed class CurrentPlaylistViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Shuffle Management

        private bool _shuffled = false;
        private List<Playlist.Member> _shuffledList;
        private List<Playlist.Member> ShuffledList => _shuffledList ?? (_shuffledList = Current?.Tracks.Shuffle());

        [EventHook("Shuffle Playlist")]
        public void OnShuffle()
        {
            _shuffled = true;
            if (Current == null) return;
            PlayOffset = ShuffledList.FindIndex(member => member.Position == Current.Tracks[PlayOffset].Position);
            RefreshDisplay();
            Dispatch("Playlist Shuffled");
        }

        [EventHook("Order Playlist")]
        public void OnOrder()
        {
            _shuffled = false;
            if (Current == null) return;
            PlayOffset = Current.Tracks.FindIndex(member => member.Position == ShuffledList[PlayOffset].Position);
            RefreshDisplay();
            _shuffledList = null;
            Dispatch("Playlist Ordered");
        }
        #endregion

        private Loop _loop = Loop.None;

        [EventHook("Disable Playlist Repeat")]
        public void OnRepeatDisable()
        {
            _loop = Loop.None;
            Dispatch("Playlist Repeat Disabled");
        }
        [EventHook("Enable Playlist Repeat")]
        public void OnRepeatEnable()
        {
            _loop = Loop.All;
            Dispatch("Playlist Repeat Enabled");
        }
        [EventHook("Enable Playlist Repeat Title")]
        public void OnRepeatTitleEnable()
        {
            _loop = Loop.One;
            Dispatch("Playlist Repeat Enabled on Track");
        }

        #region Playlist Management

        private int _playOffset = 0;
        public TrackDefinition CurrentMedia => !_shuffled ? Current?.Tracks[PlayOffset].Track : ShuffledList[PlayOffset].Track;
        public long CurrentMediaPosition => !_shuffled ? Current.Tracks[PlayOffset].Position : ShuffledList[PlayOffset].Position;

        private int PlayOffset
        {
            get { return _playOffset; }
            set
            {
                if (PlayOffset < Current.Tracks.Count && CurrentMedia != null)
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
                                PlayOffset = 0;
                                Dispatch("Play", CurrentMedia);
                                RefreshDisplay();
                                Dispatch("Stop");
                                return;
                        }
                }
                Dispatch("Play", CurrentMedia);
                RefreshDisplay();
            }
        }

        private Playlist _current;

        public Playlist Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                _shuffledList = null;
                OnPropertyChanged(nameof(Current));
            }
        }


        private readonly CollectionViewSource _currentTracks = new CollectionViewSource();

        public CurrentPlaylistViewModel()
        {
            LaunchTrack = new UiCommand(o =>
            {
                PlayOffset = _shuffled ? ShuffledList.FindIndex(track => track.Track == o as TrackDefinition) : Current.Tracks.FindIndex(track => track.Track == o as TrackDefinition);
                Dispatch("Play", CurrentMedia);
                RefreshDisplay();
            }, o => Current != null);
        }

        public ICollectionView CurrentTracks => _currentTracks.View;

        public UiCommand LaunchTrack { get; set; }

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
            _shuffledList = null;
            if (_shuffled)
                PlayOffset = ShuffledList.FindIndex(member => member.Position == Current.Tracks[PlayOffset].Position);
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
            if (_shuffled)
                PlayOffset = ShuffledList.FindIndex(member => member.Position == Current.Tracks[PlayOffset].Position);
            Current = Current;
            Dispatch("Play", CurrentMedia);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (Current.Tracks.Count > 50)
                _currentTracks.Source = !_shuffled ? Current.Tracks.Skip(PlayOffset).Take(50) : ShuffledList.Skip(PlayOffset).Take(50);
            else if (Current.Tracks.Count - PlayOffset < 50)
                _currentTracks.Source = !_shuffled ? Current.Tracks.Skip(Math.Max(0, Current.Tracks.Count - 50)) : ShuffledList.Skip(Math.Max(0, Current.Tracks.Count - 50));
            else
                _currentTracks.Source = !_shuffled ? Current.Tracks : ShuffledList;
            CurrentTracks.Refresh();
            OnPropertyChanged(nameof(CurrentTracks));
        }

        #endregion

        #region Tracks Play Control

        [EventHook("Previous Track")]
        public void PreviousTrack()
        {
            if (Current == null) return;
            if (PlayOffset - 1 >= 0)
                PlayOffset -= 1;
            else
                PlayOffset = Current.Tracks.Count - 1;
            Dispatch("Play", CurrentMedia);
            RefreshDisplay();
        }

        [EventHook("Next Track")]
        public void NextTrack()
        {
            if (Current == null) return;
            if (PlayOffset + 1 < Current.Tracks.Count)
                PlayOffset += 1;
            else
                PlayOffset = 0;
            Dispatch("Play", CurrentMedia);
            RefreshDisplay();
        }

        #endregion

        public UiCommand GoToVideos { get; } = new UiCommand(o => Dispatch("Loader: Call(My Videos)"));
        public UiCommand GoToMusics { get; } = new UiCommand(o => Dispatch("Loader: Call(My Musics)"));
    }
}