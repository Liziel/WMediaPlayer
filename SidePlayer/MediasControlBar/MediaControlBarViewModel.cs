using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DefaultMWMP2toolbar;
using DispatcherLibrary;
using SidePlayer.Annotations;
using TagLib;
using Dispatcher = DispatcherLibrary.Dispatcher;

namespace SidePlayer.MediaControlBar
{
    public enum MediaState
    {
        Play,
        Pause
    }

    public class MediaControlBarViewModel : Listener , INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Play Pause Button State

        private MediaState _mediaState = MediaState.Pause;

        public MediaState MediaState
        {
            get { return _mediaState; }
            set
            {
                _mediaState = value;
                OnPropertyChanged(nameof(MediaState));
            }
        }

        [EventHook("Media Playing")]
        public void OnMediaPlaying()
        {
            MediaState = MediaState.Play;
        }

        [EventHook("Media Paused")]
        public void OnMediaPaused()
        {
            MediaState = MediaState.Pause;
        }

        #endregion

        #region Delegates

        private UiCommand _pause = null;
        public UiCommand Pause
        {
            get { return _pause; }
            set
            {
                _pause = value;
                OnPropertyChanged(nameof(Pause));
            }
        }

        private UiCommand _play = null;
        public UiCommand Play
        {
            get { return _play; }
            set
            {
                _play = value;
                OnPropertyChanged(nameof(Play));
            }
        }

        public UiCommand Next { get; } = new UiCommand(delegate { Dispatcher.GetInstance.Dispatch("Next Track"); });
        public UiCommand Previous { get; } = new UiCommand(delegate { Dispatcher.GetInstance.Dispatch("Previous Track"); });

        #endregion

        #region Slider Property

        private double _sliderMaxValue = 100;
        public double SliderMaxValue
        {
            get { return _sliderMaxValue; }
            set
            {
                _sliderMaxValue = value;
                OnPropertyChanged(nameof(SliderMaxValue));
            }
        }

        private double _sliderCurrentValue = 0;
        public double SliderCurrentValue
        {
            get { return _sliderCurrentValue; }
            set
            {
                _sliderCurrentValue = value;
                OnPropertyChanged(nameof(SliderCurrentValue));
            }
        }

        #endregion

        #region Duration Displayer Properties

        private string _mediaDuration = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");

        public string MediaDuration
        {
            get { return _mediaDuration; }
            set
            {
                _mediaDuration = value;
                OnPropertyChanged(nameof(MediaDuration));
            }
        }

        private string _mediaPosition = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");

        public string MediaPosition
        {
            get { return _mediaPosition; }
            set
            {
                _mediaPosition = value;
                OnPropertyChanged(nameof(MediaPosition));
            }
        }

        #endregion

        #region Interface Change on Events

        [EventHook("Media Tag Created")]
        public void SliderInitialize(File file)
        {
            SliderMaxValue = file.Properties.Duration.TotalSeconds;
            MediaDuration = TimeSpan.FromSeconds(file.Properties.Duration.TotalSeconds).ToString(@"hh\:mm\:ss");
        }

        [EventHook("Media Position Actualization")]
        public void TickResponse(double position)
        {
            SliderCurrentValue = position;
            MediaPosition = TimeSpan.FromSeconds(position).ToString(@"hh\:mm\:ss");
        }

        #endregion

        #region Constructor

        public MediaControlBarViewModel()
        {
            Play = new UiCommand(delegate { Dispatcher.GetInstance.Dispatch("Play"); },
                o => MediaState == MediaState.Pause);
            Pause = new UiCommand(delegate { Dispatcher.GetInstance.Dispatch("Pause"); },
                o => MediaState == MediaState.Play);
        }

        #endregion
    }
}