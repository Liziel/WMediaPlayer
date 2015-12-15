using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaPropertiesLibrary.Audio;
using SidePlayer.Annotations;
using Dispatcher = DispatcherLibrary.Dispatcher;
using File = TagLib.File;

namespace SidePlayer.MediasPlayer.Audio
{
    public sealed class MusicPlayerPluginViewModel : Listener, INotifyPropertyChanged, IMediaPlayer
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Metadata Fields

        private MusicView _albumCoverView;
        public MusicView AlbumCoverView { get { return _albumCoverView; } set { _albumCoverView = value; OnPropertyChanged(nameof(AlbumCoverView)); } }
        public UserControl MediaView { get { return AlbumCoverView; } }

        public void OnMaximize()
        {
        }

        public void OnMinimize()
        {
            var tmp = AlbumCoverView;
            AlbumCoverView = null;
            AlbumCoverView = tmp;
        }

        private BitmapImage _albumCover = null;
        public BitmapImage AlbumCover
        {
            get { return _albumCover; }
            set
            {
                _albumCover = value;
                OnPropertyChanged(nameof(AlbumCover));
            }
        }

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
        public string MediaArtists { get { return _mediaArtists; } set { _mediaArtists = value; OnPropertyChanged(nameof(MediaArtists));} }

        private File _tag;

        private void InitializeCover()
        {
            if (_tag.Tag.IsEmpty || _tag.Tag.Pictures.Length == 0)
                return;

            var picture = _tag.Tag.Pictures[0];
            MemoryStream mstream = new MemoryStream(picture.Data.Data);
            mstream.Seek(0, SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = mstream;
            bitmap.EndInit();

            AlbumCover = bitmap;
        }

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

        private MediaElement _music;

        public MediaElement Music
        {
            get { return _music; }
            set
            {
                _music = value;
                OnPropertyChanged(nameof(Music));
            }
        }

        [EventHook("Play")]
        public void Play()
        {
            _music.Play();
            _tick.Start();
            Dispatcher.GetInstance.Dispatch("Media Playing");
        }

        [EventHook("Pause")]
        public void Pause()
        {
            _music.Pause();
            _tick.Stop();
            Dispatcher.GetInstance.Dispatch("Media Paused");
        }

        [EventHook("Stop")]
        public void Stop()
        {
            _music.Stop();
            _tick.Stop();
        }

        [EventHook("Media Position Set")]
        public void ForceSetPosition(double duration)
        {
            _music.Position = TimeSpan.FromSeconds(duration);
        }
        #endregion

        #region Media Posiion Fields

        private DispatcherTimer _tick = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};

        private void OnTick(object sender, EventArgs e)
        {
            Dispatcher.GetInstance.Dispatch("Media Position Actualization", _music.Position.TotalSeconds);
        }

        #endregion

        #region Constructor

        public MusicPlayerPluginViewModel()
        {
            AlbumCoverView = new MusicView(this);
            Music = new MediaElement { LoadedBehavior = MediaState.Manual, UnloadedBehavior = MediaState.Manual };
        }

        public void AssignUri(Uri media, TagLib.File tag)
        {
            _tag = tag;

            InitializeCover();
            InializeTitle(Path.GetFileNameWithoutExtension(media.LocalPath));

            _tick.Tick += OnTick;
        }

        public void AssignMedia(object media)
        {
            Track track = media as Track;

            Music.Source = new Uri(track.Path);
            ForceSetPosition(0);
            AlbumCover = track.Album?.Cover;

            MediaName = track.Name;
            _tick.Tick += OnTick;
        }
        #endregion
    }
}