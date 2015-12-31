using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using SidePlayer.Annotations;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace SidePlayer.MediasControlBar
{
    public enum RepeatState
    {
        None, One, Forever
    }

    public sealed class SubControlViewModel : Listener, INotifyPropertyChanged
    {
        #region Volume Control

        private double _volume = 1;

        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
                Dispatch("Media Volume Set", value);
            }
        }

        private bool _mute = false;

        public bool Mute
        {
            get { return _mute; }
            set
            {
                _mute = value;
                OnPropertyChanged(nameof(Mute));
                Dispatch("Media Volume Set", !value ? Volume : 0.0);
            }
        }

        #endregion

        #region Playlist States

        private bool _shuffle = false;
        public bool Shuffle
        {
            get { return _shuffle; }
            set
            {
                _shuffle = value;
                OnPropertyChanged(nameof(Shuffle));
            }
        }

        private RepeatState _repeatState;
        public RepeatState Repeat
        {
            get { return _repeatState; }
            set
            {
                _repeatState = value;
                OnPropertyChanged(nameof(Repeat));
            }
        }

        #endregion

        #region Playlist Commands

        public UiCommand RepeatCommand { get; }
        public UiCommand ShuffleCommand { get; }

        [EventHook("Playlist Shuffled")]
        public void OnPlaylistShuffled() => Shuffle = true;
        [EventHook("Playlist Ordered")]
        public void OnPlaylistOrdered() => Shuffle = false;

        [EventHook("Playlist Repeat Enabled")]
        public void OnPlaylistState2() => Repeat = RepeatState.Forever;
        [EventHook("Playlist Repeat Disabled")]
        public void OnPlaylistState0() => Repeat = RepeatState.None;
        [EventHook("Playlist Repeat Enabled on Track")]
        public void OnPlaylistState1() => Repeat = RepeatState.One;
        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public SubControlViewModel()
        {
            AddEventListener(this);
            ShuffleCommand = new UiCommand(o =>
            {
                switch (!Shuffle)
                {
                    case true:
                        Dispatch("Shuffle Playlist");
                        break;
                    default:
                        Dispatch("Order Playlist");
                        break;
                }
            });
            RepeatCommand = new UiCommand(o =>
            {
                switch (Repeat)
                {
                    case RepeatState.None:
                        Dispatch("Enable Playlist Repeat");
                        break;
                    case RepeatState.Forever:
                        Dispatch("Enable Playlist Repeat Title");
                        break;
                    case RepeatState.One:
                        Dispatch("Disable Playlist Repeat");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                OnPropertyChanged(nameof(Repeat));
            });
        }
    }
}