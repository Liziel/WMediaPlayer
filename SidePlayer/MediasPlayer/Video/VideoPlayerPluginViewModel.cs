using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaPropertiesLibrary.Video;
using SidePlayer.Annotations;
using TagLib.Matroska;
using static DispatcherLibrary.Dispatcher;
using File = TagLib.File;
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

        private MediaElement _video = new MediaElement {LoadedBehavior = MediaState.Manual, UnloadedBehavior = MediaState.Manual};

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
            _senderTick.Start();
            Dispatch("Media Playing");
        }

        [EventHook("Pause")]
        public void Pause()
        {
            _video.Pause();
            _senderTick.Stop();
            Dispatch("Media Paused");
        }

        [EventHook("Stop")]
        public void Stop()
        {
            _video.Stop();
            _senderTick.Stop();
            _subtitleTick.Stop();
        }
        [EventHook("Media Position Set")]
        public void ForceSetPosition(double duration)
        {
            _video.Position = TimeSpan.FromSeconds(duration);
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

        #region Subtitles Loading

        private void LoadSubtitles(Uri media)
        {
            TagLib.File f = TagLib.File.Create(media.LocalPath);
            bool hasSubtitle = false;

            foreach (var codec in f.Properties.Codecs)
                if (codec is SubtitleTrack)
                    hasSubtitle = true;
            if (!hasSubtitle)
                return;
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"ffmpeg",
                    Arguments = "-i \"" + media.LocalPath + "\" -y -an -vn -threads 4 -c:s:0 srt .currently_playing.srt",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };


            _process.Exited += OnProcessOnExited;

            _process.EnableRaisingEvents = true;
            _process.Start();
        }

        private Process _process;

        private void OnProcessOnExited(object o, EventArgs args)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                lock (_subtitles)
                    _subtitles.UpdateSubtitles(new Uri("./.currently_playing.srt", UriKind.Relative));
                lock (_subtitles)
                    _subtitleTick.Start();

            });
            _process.Close();
            _process.Exited -= OnProcessOnExited;
            _process = null;
        }

        #endregion

        #region Constructor

        public VideoPlayerPluginViewModel()
        {
            SubtitleView = new SubtitleView(_subtitles);
            VideoView = new VideoView(this);

            _senderTick.Tick += OnSenderTick;
            _subtitleTick.Tick += RefreshSubtitles;
        }

        public void AssignUri(Uri media, File tag)
        {
            if (_process != null)
            {
                _process.Exited -= OnProcessOnExited;
                _process.Kill();
            }
            Video.Source = media;
            _tag = tag;
            LoadSubtitles(media);
            InializeTitle(Path.GetFileNameWithoutExtension(media.LocalPath));
        }

        public void AssignMedia(object media)
        {
            Track track = media as Track;

            if (track == null)
                return;
            _process?.Kill();
            _subtitles.Clear();
            var trackUri = new Uri(track.Path);
            Video.Source = trackUri;
            LoadSubtitles(trackUri);
            ForceSetPosition(0);
            MediaName = track.Name;
        }

        #endregion
    }
}