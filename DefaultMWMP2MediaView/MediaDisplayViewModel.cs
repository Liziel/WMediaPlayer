using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using DefaultMWMP2MediaView.Annotations;
using SharedDispatcher;

namespace DefaultMWMP2MediaView
{
    public class MediaDisplayViewModel : Listener, INotifyPropertyChanged
    {
        #region MediaElement Initialization

        private MediaElement _mediaElement;

        public MediaElement MediaElementObject
        {
            get { return _mediaElement; }
            set
            {
                _mediaElement = value;
                OnPropertyChanged(nameof(MediaElement));
            }
        }

        public MediaDisplayViewModel(Uri source)
        {
            _mediaElement = new MediaElement {Source = source, LoadedBehavior = MediaState.Manual};
            Play();
        }

        #endregion

        #region Notifier Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Media Controls

        [EventHook("Play")]
        public void Play()
        {
            MediaElementObject.Play();
            Dispatcher.GetInstance.Dispatch("Media Playing");
        }

        [EventHook("Pause")]
        public void Pause()
        {
            MediaElementObject.Pause();
            Dispatcher.GetInstance.Dispatch("Media Paused");
        }

        #endregion
    }
}