using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Audio.Library;
using SidePlayer.Annotations;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

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
        public UserControl MediaView => AlbumCoverView;

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

        public TextBlock ArtistsM { get; set; } = null;
        public TextBlock ArtistsP { get; set; } = null;
        public TextBlock ArtistsV { get; set; } = null;

        #endregion

        #region Music Fields

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
            _track.State = MediaPropertiesLibrary.MediaState.Playing;
            _tick.Start();
            Dispatch("Media Playing");
            if (_track.Artists.Count > 0)
                Dispatch("Current Media Name", _track.Artists[0].Name + " - " + _track.Name);
            else
                Dispatch("Current Media Name", _track.Name);
        }

        [EventHook("Pause")]
        public void Pause()
        {
            _music.Pause();
            _track.State  = MediaPropertiesLibrary.MediaState.Paused;
            _tick.Stop();
            Dispatch("Media Paused");
        }

        [EventHook("Stop")]
        public void Stop()
        {
            _music.Stop();
            _track.State = MediaPropertiesLibrary.MediaState.Stopped;
            _tick.Stop();
            Dispatch("Media Stopped");
        }

        [EventHook("Media Position Set")]
        public void ForceSetPosition(double duration)
        {
            _music.Position = TimeSpan.FromSeconds(duration);
        }

        [EventHook("Media Volume Set")]
        public void MediaVolumeSet(double volume)
        {
            _music.Volume = volume;
        }
        #endregion

        #region Media Posiion Fields

        private DispatcherTimer _tick = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};

        private void OnTick(object sender, EventArgs e)
        {
            Dispatch("Media Position Actualization", _music.Position.TotalSeconds);
        }

        #endregion

        #region Constructor

        public MusicPlayerPluginViewModel()
        {
            AlbumCoverView = new MusicView(this);
            Music = new MediaElement { LoadedBehavior = MediaState.Manual, UnloadedBehavior = MediaState.Manual };
            Music.MediaEnded += (o, p) =>
            {
                _track.UserTag.TimesListened += 1;
                _track.State = MediaPropertiesLibrary.MediaState.End;
                Library.Save();
            };
        }

        private Track _track;

        public void AssignMedia(object media)
        {
            Track track = media as Track;
            _track = track;
            Music.Source = new Uri(track.Path);
            ForceSetPosition(0);
            AlbumCover = track.Album?.Cover;

            if (_track.Artists.Count > 0)
                CreateArtistBand(track);
            MediaName = track.Name;

            _tick.Tick -= OnTick;
            _tick.Tick += OnTick;

            AccessAlbum = new UiCommand(o =>
            {
                Dispatch("Loader: Call(My Musics)");
                Dispatch("AudioLibrary: View Album", track.Album);
            });
        }

        private UiCommand _accessAlbum;
        public UiCommand AccessAlbum
        {
            get { return _accessAlbum; }
            set
            {
                _accessAlbum = value; 
                OnPropertyChanged(nameof(AccessAlbum));
            }
        }

        private void CreateArtistBand(Track track)
        {
            if (track.Artists == null)
                return;
            ArtistsM = new TextBlock
            {
                FontWeight = FontWeights.Black,
                FontSize = 14,
                FontStretch = FontStretches.UltraExpanded,
                Foreground = Brushes.LightGray
            };
            ArtistsP = new TextBlock
            {
                FontWeight = FontWeights.Black,
                FontSize = 14,
                FontStretch = FontStretches.UltraExpanded,
                Foreground = Brushes.LightGray
            };
            ArtistsV = new TextBlock
            {
                FontWeight = FontWeights.Black,
                FontSize = 14,
                FontStretch = FontStretches.UltraExpanded,
                Foreground = Brushes.LightGray
            };
            var tmp = track.Artists.Last();
            foreach (var artist in track.Artists)
            {
                ArtistsM.Inlines.Add(new Hyperlink
                {
                    Command = new UiCommand(o =>
                    {
                        Dispatch("Loader: Call(My Musics)");
                        Dispatch("AudioLibrary: View Artist", artist);
                    }),
                    Foreground = Brushes.LightGray,
                    TextDecorations = null,
                    Inlines = { artist.Name }
                });
                if (artist != tmp)
                    ArtistsM.Inlines.Add(", ");
                ArtistsP.Inlines.Add(new Hyperlink
                {
                    Command = new UiCommand(o =>
                    {
                        Dispatch("Loader: Call(My Musics)");
                        Dispatch("AudioLibrary: View Artist", artist);
                    }),
                    Foreground = Brushes.LightGray,
                    TextDecorations = null,
                    Inlines = { artist.Name }
                });
                if (artist != tmp)
                    ArtistsP.Inlines.Add(", ");
                ArtistsV.Inlines.Add(new Hyperlink
                {
                    Command = new UiCommand(o =>
                    {
                        Dispatch("Loader: Call(My Musics)");
                        Dispatch("AudioLibrary: View Artist", artist);
                    }),
                    Foreground = Brushes.LightGray,
                    TextDecorations = null,
                    Inlines = { artist.Name }
                });
                if (artist != tmp)
                    ArtistsV.Inlines.Add(", ");
            }
            OnPropertyChanged(nameof(ArtistsM));
            OnPropertyChanged(nameof(ArtistsP));
            OnPropertyChanged(nameof(ArtistsV));
        }
        #endregion
    }
}