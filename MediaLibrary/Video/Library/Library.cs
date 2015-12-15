using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaPropertiesLibrary.Video;
using PluginLibrary;

namespace MediaLibrary.Video.Library
{
    public delegate void OnAllTracksLoaded();

    public class Library
    {
        #region Tracks Loaded Event

        public static event OnAllTracksLoaded TracksLoaded;
        private static void OnTracksLoaded()
        {
            TracksLoaded?.Invoke();
        }

        #endregion

        #region Tracks & Series

        private readonly List<Track> _tracks = new List<Track>();
        private readonly List<Serie> _series = new List<Serie>();

        public static List<Track> Tracks
        {
            get { lock (Instance._tracks) return Instance._tracks; }
        }

        public static List<Serie> Series
        {
            get { lock (Instance._series) return Instance._series; }
        }

        #endregion

        #region User Defined Parts

        private static List<UserTrackDefinition> CreateUserTracksDefintions(string userTrackDefinitionsFile)
        {
            Stream userTrackDefinitionsStream = null;
            try
            {
                using (userTrackDefinitionsStream = new FileStream(userTrackDefinitionsFile, FileMode.OpenOrCreate))
                    return (List<UserTrackDefinition>) new XmlSerializer(typeof(List<UserTrackDefinition>)).Deserialize(userTrackDefinitionsStream);
            }
            catch (IOException)
            {
                using (userTrackDefinitionsStream = new FileStream(userTrackDefinitionsFile, FileMode.Open))
                    return (List<UserTrackDefinition>) new XmlSerializer(typeof(List<UserTrackDefinition>)).Deserialize(userTrackDefinitionsStream);
            }
        }

        private static void SaveUserTracksDefinitions(string userTrackDefinitionsFile)
        {
            Stream userTrackDefinitionsStream = null;
            using (userTrackDefinitionsStream = new FileStream(userTrackDefinitionsFile, FileMode.OpenOrCreate))
                new XmlSerializer(typeof (List<UserTrackDefinition>)).Serialize(userTrackDefinitionsStream,
                Instance._userTracksDefinitions);
        }

        private readonly List<UserTrackDefinition> _userTracksDefinitions
            =
            CreateUserTracksDefintions(AbstractPathLibrary.LibrariesLocation + "/UserVideoTracksDefinitions.xml");

        private static List<UserSerieDefinition> CreateUserSeriesDefintions(string userTrackDefinitionsFile)
        {
            Stream userTrackDefinitionsStream = null;
            try
            {
                using (userTrackDefinitionsStream = new FileStream(userTrackDefinitionsFile, FileMode.OpenOrCreate))
                    return (List<UserSerieDefinition>)
                    new XmlSerializer(typeof(List<UserSerieDefinition>)).Deserialize(userTrackDefinitionsStream);
            }
            catch (IOException)
            {
                using (userTrackDefinitionsStream = new FileStream(userTrackDefinitionsFile, FileMode.Open))
                    return (List<UserSerieDefinition>)
                    new XmlSerializer(typeof(List<UserSerieDefinition>)).Deserialize(userTrackDefinitionsStream);
            }
        }

        private static void SaveUserSeriesDefinitions(string userTrackDefinitionsFile)
        {
            Stream userTrackDefinitionsStream = null;
            using (userTrackDefinitionsStream = new FileStream(userTrackDefinitionsFile, FileMode.OpenOrCreate))
                new XmlSerializer(typeof(List<UserSerieDefinition>)).Serialize(userTrackDefinitionsStream,
                        Instance._userSeriesDefinitions);
        }


        private readonly List<UserSerieDefinition> _userSeriesDefinitions
            =
            CreateUserSeriesDefintions(AbstractPathLibrary.LibrariesLocation + "/UserVideoSeriesDefinitions.xml");

        #endregion

        #region Initializer

        private static Library Instance = new Library();

        private Library()
        {
        }

        internal static void Initialize()
        {
            foreach (var userSeriesDefinition in Instance._userSeriesDefinitions)
                Instance._series.Add(new Serie
                {
                    Cover =
                        string.IsNullOrEmpty(userSeriesDefinition.CoverPath)
                            ? null
                            : new BitmapImage(new Uri(userSeriesDefinition.CoverPath)),
                    Name = userSeriesDefinition.Name
                });
            foreach (var userTracksDefinition in Instance._userTracksDefinitions)
                Instance._tracks.Add(new Track
                {
                    UserTrackDefinition = userTracksDefinition,
                    Name = userTracksDefinition.Name
                });
            new Thread(new ThreadStart(delegate
            {
                PathLibrary.Synchronize(new Dictionary<string, Action<List<string>, string>>
                {
                    {"*.mp4", Instance.OnFoundFile},
                    {"*.avi", Instance.OnFoundFile},
                    {"*.mkv", Instance.OnFoundFile},
                });
                Tracks.Remove(Tracks.Single(track => string.IsNullOrEmpty(track.Path)));
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
            Serie serie = null;
            if (!metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Album))
            {
                serie = _series.Find(searchedSerie => searchedSerie.Name == metaData.Tag.Album);
                if (serie == null)
                {
                    var def =
                        _userSeriesDefinitions.Find(searchedDefinition => searchedDefinition.Name == metaData.Tag.Album);
                    lock (_series)
                        _series.Add(
                            serie =
                                new Serie
                                {
                                    Name = metaData.Tag.Album,
                                    Cover = def == null ? null : new BitmapImage(new Uri(def.CoverPath))
                                });
                }
            }
            Track track = new Track
            {
                Name =
                    !metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Title)
                        ? metaData.Tag.Title
                        : Path.GetFileNameWithoutExtension(path),

                Path = path,
                RelativePath = relativePath,

                Duration = metaData.Properties.Duration,

                Serie = serie,
            };
            lock(_tracks)
                _tracks.Add(track);
            track.UserTrackDefinition =
                _userTracksDefinitions.Find(searchedDefinition => searchedDefinition.Name == track.Name);
            if (track.UserTrackDefinition != null && track.Serie == null)
            {
                track.Serie =
                    _series.Find(searchedDefinition => searchedDefinition.Name == track.UserTrackDefinition.Name);
            }
        }

        #endregion
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