using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;
using DispatcherLibrary;
using SidePlayer.Annotations;
using Dispatcher = DispatcherLibrary.Dispatcher;

namespace SidePlayer.MediasPlayer.Video
{
    public class VideoPlayerPluginViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Metadata Fields

        private string _mediaName = "";
        public string MediaName
        {
            get { return _mediaName; }
            set
            {
                _mediaName = value;
                OnPropertyChanged(nameof(MediaName));
            }
        }

        private string _mediaArtists = "";
        public string MediaArtists { get { return _mediaArtists; } set { _mediaArtists = value; OnPropertyChanged(nameof(MediaArtists)); } }

        private TagLib.File _tag;

        private void InializeTitle(string filename)
        {
            if (!_tag.Tag.IsEmpty && !string.IsNullOrEmpty(_tag.Tag.Title))
                MediaName = _tag.Tag.Title;
            else
                MediaName = filename;

            if (!_tag.Tag.IsEmpty)
                MediaArtists = string.Join(", ", _tag.Tag.Performers);
        }

        #endregion

        #region Video Fields

        private VideoView _videoView;
        public VideoView VideoView { get { return _videoView; } set { _videoView = value; OnPropertyChanged(nameof(VideoView)); } }

        private MediaElement _video;

        public MediaElement Video
        {
            get { return _video; }
            set
            {
                _video = value;
                OnPropertyChanged(nameof(Video));
            }
        }

        [EventHook("Play")]
        public void Play()
        {
            if (Video.LoadedBehavior == MediaState.Pause)
                Video.LoadedBehavior = MediaState.Manual;
            _video.Play();
            _tick.Start();
            Dispatcher.GetInstance.Dispatch("Media Playing");
        }

        [EventHook("Pause")]
        public void Pause()
        {
            _video.Pause();
            _tick.Stop();
            Dispatcher.GetInstance.Dispatch("Media Paused");
        }

        [EventHook("Media Position Set")]
        public void ForceSetPosition(double duration)
        {
            _video.Position = TimeSpan.FromSeconds(duration);
        }
        #endregion

        #region Media Posiion Fields

        private DispatcherTimer _tick = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        private void OnTick(object sender, EventArgs e)
        {
            Dispatcher.GetInstance.Dispatch("Media Position Actualization", _video.Position.TotalSeconds);
        }

        #endregion

        #region Constructor

        public VideoPlayerPluginViewModel(Uri media, TagLib.File tag)
        {
            VideoView = new VideoView(this);
            Video = new MediaElement { Source = media, LoadedBehavior = MediaState.Pause, ScrubbingEnabled = true};
            _tag = tag;

            InializeTitle(Path.GetFileNameWithoutExtension(media.LocalPath));

            _tick.Tick += OnTick;
        }

        #endregion
    }
}