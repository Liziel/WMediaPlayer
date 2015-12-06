using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using SharedDispatcher;
using SidePlayer.Annotations;

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

        private MediaState _mediaState = MediaState.Pause;
        public MediaState MediaState{ get { return _mediaState; } set { _mediaState = value; OnPropertyChanged(nameof(MediaState)); } }

        [EventHook("Media Playing")]
        public void OnMediaPlaying() { MediaState = MediaState.Play; }

        [EventHook("Media Paused")]
        public void OnMediaPaused() { MediaState = MediaState.Pause; }
    }
}