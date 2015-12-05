using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using DefaultMWMP2toolbar.Annotations;
using SharedDispatcher;

namespace DefaultMWMP2toolbar
{
    public class ClassicToolbarModelView : Listener, INotifyPropertyChanged
    {
        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Control Delegates

        private UiCommand _play;
        private UiCommand _pause;

        private UiCommand _playpause;
        public UiCommand PlayPause { get { return _playpause; } set { _playpause = value; OnPropertyChanged(nameof(PlayPause)); } }
        private UiCommand _stop;
        public UiCommand Stop { get { return _stop; } set { _stop = value; OnPropertyChanged(nameof(Stop)); } }

        public ClassicToolbarModelView()
        {
            Stop = new UiCommand(o => this.StopMedia());
            _play = new UiCommand(o => this.Play());
            _pause = new UiCommand(o => this.Pause());
            PlayPause = _play;
        }

        #endregion

        #region Control Methods

        void Play()
        {
            Dispatcher.GetInstance.Dispatch("Play");
        }

        void Pause()
        {
            Dispatcher.GetInstance.Dispatch("Pause");
        }

        void StopMedia()
        {
            Dispatcher.GetInstance.Dispatch("Stop");
        }

        #endregion

        #region PlayPause Button Management

        private String _imageSource = "Textures/play.png";

        public String ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        [EventHook("Media Playing")]
        public void SetPauseButton()
        {
            ImageSource = "Textures/pause.png";
            PlayPause = _pause;
        }

        [EventHook("Media Paused")]
        public void SetPlayButton()
        {
            ImageSource = "Textures/play.png";
            PlayPause = _play;
        }

        #endregion
    }
}