using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using PluginLibrary;
using TagLib.Matroska;
using File = System.IO.File;

namespace MediaPropertiesLibrary.Video.Library
{
    public class TrackSerializer
    {
        public Track Track { get; set; }
        public string SerieName { get; set; }
    }

    public class Library
    {
        #region Serializable

        private static string VideoLibraryLocation => Locations.Libraries + "/SavedVideoLibrary.xml";

        private List<TrackSerializer> _useForTrackDeserializer;

        [XmlElement("LastSubtitleTag", Order = 0)]
        public long LastSubtitleTag { get; set; }

        [XmlArray("Videos", Order = 2)]
        public List<TrackSerializer> SerializableTracks
        {
            get
            {
                if (_tracks == null)
                    return _useForTrackDeserializer = new List<TrackSerializer>();
                return _tracks.Select(track => new TrackSerializer
                {
                    Track = track,
                    SerieName = track.Serie?.Name
                }).ToList();
            }
        }

        [XmlArray("Series", Order = 1)]
        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public ObservableCollection<Serie> SerializableSeries => _series;

        #endregion

        #region Videos & Series

        private ObservableCollection<Track> _tracks = null;
        private readonly ObservableCollection<Serie> _series = new ObservableCollection<Serie>(new List<Serie>());

        public static ObservableCollection<Track> Videos { get { lock (Instance._tracks) return Instance._tracks; } }
        public static ObservableCollection<Serie> Series { get { lock (Instance._series) return Instance._series; } }

        #endregion

        private static void OnTracksLoaded()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (Videos == null) return;
                foreach (var workingTrack in Instance._workingTracks)
                    Videos.Add(workingTrack);
                Instance._working = false;

                using (var stream = new FileStream(VideoLibraryLocation, FileMode.Truncate))
                    new XmlSerializer(typeof(Library)).Serialize(stream, Instance);
            });
        }

        #region Initializer

        private static Library CreateInstance()
        {
            Library instance;
            try
            {
                using (var stream = new FileStream(VideoLibraryLocation, FileMode.OpenOrCreate))
                    instance = (Library) new XmlSerializer(typeof (Library)).Deserialize(stream);
            }
            catch (Exception)
            {
                instance = new Library();
            }
            instance._tracks = new ObservableCollection<Track>();
            foreach (
                var trackSerializer in
                    instance._useForTrackDeserializer.Where(
                        trackSerializer => File.Exists(trackSerializer.Track.Path)))
            {
                var serie = instance._series.FirstOrDefault(s => s.Name == trackSerializer.SerieName);
                serie?.Tracks.Add(trackSerializer.Track);
                instance._tracks.Add(trackSerializer.Track);
            }
            foreach (var serie in instance._series.Where(serie => serie.Tracks.Count == 0).ToList())
                instance._series.Remove(serie);
            return instance;
        }

        private static readonly Library Instance = CreateInstance();

        private Library()
        {
        }

        private readonly List<Track> _workingTracks = new List<Track>();
        private bool _working = true;
        internal static void Initialize()
        {
            new Thread(new ThreadStart(delegate
            {
                PathLibrary.Synchronize(new Dictionary<string, Action<List<string>, string>>
                {
                    {"*.mp4", Instance.OnFoundFile},
                    {"*.avi", Instance.OnFoundFile},
                    {"*.mkv", Instance.OnFoundFile},
                });
                OnTracksLoaded();
            })).Start();
        }

        public void OnFoundFile(List<string> relativePath, string path)
        {
            TagLib.File metaData = null;
            try
            {
                metaData = TagLib.File.Create(path);
            }
            catch (Exception e)
            {
                return;
            }
            var track = _workingTracks.FirstOrDefault(t => t.Path == path) ??
                          _tracks.FirstOrDefault(t => t.Path == path);
            if (track == null)
            {
                track = new Track
                {
                    Name =
                        !metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Title)
                            ? metaData.Tag.Title
                            : Path.GetFileNameWithoutExtension(path),

                    Path = path,
                    RelativePath = relativePath,

                    Duration = metaData.Properties.Duration,
                };
                _workingTracks.Add(track);
            }

            var hasSubtitle = false;
            foreach (var codec in metaData.Properties.Codecs.OfType<SubtitleTrack>())
                hasSubtitle = true;
            if (!hasSubtitle || track.Subtitles.Any(subtitle => subtitle.Name == "encoded subtitles"))
                return;
            if (!Directory.Exists(Locations.DataFolder + "/subtitles"))
                Directory.CreateDirectory(Locations.DataFolder + "/subtitles");
            var subtitleLocation = Locations.DataFolder + "/subtitles/inner_subtitle" + LastSubtitleTag + ".srt";
            var _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"ffmpeg",
                    Arguments = "-i \"" + track.Path + "\" -y -an -vn -threads 1 -c:s:0 srt \"" + subtitleLocation + "\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };
            LastSubtitleTag += 1;
            _process.Exited += (sender, args) =>
            {
                track.Subtitles.Add(new Subtitle {Name = "encoded subtitles", Path = subtitleLocation});
                Save();
            };
            _process.EnableRaisingEvents = true;
            _process.Start();
        }

        #endregion

        public static void Save()
        {
            if (Instance._working) return;
            lock (Instance)
                using (var audioLibraryStream = new FileStream(VideoLibraryLocation, FileMode.Truncate))
                    new XmlSerializer(Instance.GetType()).Serialize(audioLibraryStream, Instance);
        }

    }

    [Export(typeof (IStaticRessource))]
    public class VideoLibraryInstantiator : IStaticRessource
    {
        public VideoLibraryInstantiator()
        {
            Library.Initialize();
        }

        public void Initialize()
        {
        }
    }
}