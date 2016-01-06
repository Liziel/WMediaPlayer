using System.ComponentModel;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using SidePlayer.Annotations;
using SidePlayer.MaximizedMediaPlayer;
using SidePlayer.MediaControlBar;
using SidePlayer.MediasPlayer;
using static DispatcherLibrary.Dispatcher;

namespace SidePlayer
{
    public class SidePlayerPluginMainViewModel : Listener, INotifyPropertyChanged
    {
        #region MediaViewer and MediaControlBar

        private IMediaPlayer _mediaViewer;

        [ForwardDispatch]
        public IMediaPlayer MediaViewer
        {
            get { return _mediaViewer; }
            set
            {
                _mediaViewer = value;
                OnPropertyChanged(nameof(MediaViewer));
            }
        }

        private MediaControlBarViewModel _mediaControlBar;

        [ForwardDispatch]
        public MediaControlBarViewModel MediaControlBar
        {
            get { return _mediaControlBar; }
            set
            {
                _mediaControlBar = value;
                OnPropertyChanged(nameof(MediaControlBar));
            }
        }

        #endregion

        #region Notifier Properties and Methods

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private MaximizedMediaPlayerView _maximizedView = null;

        [EventHook("Maximize Media View")]
        public void MediaMaximize()
        {
            MediaViewer.OnMaximize();
            _maximizedView =
                new MaximizedMediaPlayerView(new MaximizedMediaPlayerViewModel(MediaViewer.MediaView,
                    new MaximizedMediaControlView(MediaControlBar)));
            Dispatch("Attach Plugin On Top", _maximizedView);
            Dispatch("View: Fullscreen");
        }

        [EventHook("Minimize Media View")]
        public void MediaMinimize()
        {
            Dispatch("Remove Plugin", _maximizedView);
            _maximizedView = null;
            System.GC.Collect();
            MediaViewer.OnMinimize();
            Dispatch("View: Normal");
        }

        public SidePlayerPluginMainViewModel(IMediaPlayer mediaPlayer)
        {
            MediaViewer = mediaPlayer;
            MediaControlBar = new MediaControlBarViewModel();
        }

        public void AssignMedia(object media)
        {
            MediaViewer.AssignMedia(media);
        }
    }
}