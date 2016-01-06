using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaPropertiesLibrary.Video;
using SidePlayer.Annotations;
using TagLib.Matroska;
using static DispatcherLibrary.Dispatcher;
using Track = MediaPropertiesLibrary.Video.Track;

namespace SidePlayer.MediasPlayer.Video
{
    public sealed class VideoPlayerPluginViewModel : Listener, INotifyPropertyChanged, IMediaPlayer
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
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

        private string _serieName = "";
        public string SerieName { get { return _serieName; } set { _serieName = value; OnPropertyChanged(nameof(SerieName)); } }

        private TagLib.File _tag;

        private void InializeTitle(string filename)
        {
            if (!_tag.Tag.IsEmpty && !string.IsNullOrEmpty(_tag.Tag.Title))
                MediaName = _tag.Tag.Title;
            else
                MediaName = filename;

            if (!_tag.Tag.IsEmpty)
                SerieName = string.Join(", ", _tag.Tag.Performers);
        }

        #endregion

        #region Video Fields

        private VideoView _videoView;
        public VideoView VideoView { get { return _videoView; } set { _videoView = value; OnPropertyChanged(nameof(VideoView)); } }
        public UserControl MediaView => VideoView;

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
            _video.Play();
            _track.State = MediaPropertiesLibrary.MediaState.Playing;
            _senderTick.Start();
            Dispatch("Media Playing");
            Dispatch("Current Media Name", _track.Name);
        }

        [EventHook("Pause")]
        public void Pause()
        {
            _video.Pause();
            _track.State = MediaPropertiesLibrary.MediaState.Paused;
            _senderTick.Stop();
            Dispatch("Media Paused");
        }

        [EventHook("Stop")]
        public void Stop()
        {
            _video.Stop();
            _track.State = MediaPropertiesLibrary.MediaState.Stopped;
            _senderTick.Stop();
            _subtitleTick.Stop();
        }
        [EventHook("Media Position Set")]
        public void ForceSetPosition(double duration)
        {
            _video.Position = TimeSpan.FromSeconds(duration);
        }

        [EventHook("Media Volume Set")]
        public void MediaVolumeSet(double volume)
        {
            _video.Volume = volume;
        }

        private bool _maximized = false;
        public bool Maximized { get { return _maximized; } set { _maximized = value; OnPropertyChanged(nameof(Maximized)); } }

        public void OnMaximize()
        {
            Maximized = true;
        }

        public void OnMinimize()
        {
            {
                var tmp = VideoView;
                VideoView = null;
                VideoView = tmp;
            }
            Maximized = false;
        }

        #endregion

        #region Media Posiion Fields

        private readonly DispatcherTimer _senderTick = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

        private void OnSenderTick(object sender, EventArgs e)
        {
            Dispatch("Media Position Actualization", _video.Position.TotalSeconds);
        }

        #endregion

        #region Subtitles

        private readonly DispatcherTimer _subtitleTick = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };

        private Subtitles _subtitles = new Subtitles();

        private SubtitleView _subtitleViewDisplay;
        public SubtitleView SubtitleView
        {
            get
            {
                return _subtitleViewDisplay;
            }
            set
            {
                _subtitleViewDisplay = value;
                OnPropertyChanged(nameof(SubtitleView));
            }
        }

        public void RefreshSubtitles(object sender, EventArgs eventArgs)
        {
            lock(_subtitles)
                if (_subtitles != null)
                    _subtitles.Refresh(_video.Position);
        }

        #endregion


        #region Constructor

        public VideoPlayerPluginViewModel()
        {
            _video = new MediaElement
            {
                LoadedBehavior = MediaState.Manual, UnloadedBehavior = MediaState.Manual, Stretch = Stretch.Uniform
                
            };
            SubtitleView = new SubtitleView(_subtitles);
            VideoView = new VideoView(this);
            _video.MediaEnded += (o, p) =>
            {
                _track.State = MediaPropertiesLibrary.MediaState.End;
            };

            _senderTick.Tick += OnSenderTick;
            _subtitleTick.Tick += RefreshSubtitles;
        }

        private Track _track;

        public void AssignMedia(object media)
        {
            var track = media as Track;
            _track = track;
            if (track == null)
                return;
            Video.Source = new Uri(track.Path);
            ForceSetPosition(0);
            MediaName = track.Name;
        }

        #endregion
    }
}