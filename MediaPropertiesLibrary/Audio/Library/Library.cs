using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using PluginLibrary;
using File = TagLib.File;

namespace MediaPropertiesLibrary.Audio.Library
{ 

    public delegate void OnTracksActualized();

    [Serializable]
    public class TrackSerializer
    {
        public Track Track { get; set; }
        public string AlbumName { get; set; }
        public List<string> ArtistsNames { get; set; }
    }

    public class Library
    {
        private static string AudioLibraryLocation => AbstractPathLibrary.LibrariesLocation + "/SavedAudioLibrary.xml";

        private List<TrackSerializer> useForDeserializer;

        [XmlArray("Tracks", Order = 2)]
        public List<TrackSerializer> SerializableTracks {
            get
            {
                if (_tracks == null)
                    return useForDeserializer = new List<TrackSerializer>();
                return _tracks.Select(track => new TrackSerializer
                {
                    Track = track,
                    AlbumName = track.Album?.Name,
                    ArtistsNames = track.Artist.Select(artist => artist.Name).ToList()
                }).ToList();
            }
        }
        [XmlArray("Albums", Order = 1)]
        public List<Album> SerializableAlbums { get { return _albums; } set { _albums = value; } }
        [XmlArray("Artists", Order = 0)]
        public List<Artist> SerializableArtists { get { return _artists; } set { _artists = value; } }

        #region Library Tracks Artists & Albums

        private List<Track> _tracks = null;
        private List<Album> _albums = new List<Album>();
        private List<Artist> _artists = new List<Artist>();

        private List<Track> _workingtracks = new List<Track>();
        private List<Album> _workingalbums = new List<Album>();
        private List<Artist> _workingartists = new List<Artist>();

        public static List<Track> Tracks
        {
            get
            {
                lock (Instance._tracks)
                {
                    return Instance._tracks;
                }
            }
        }

        public static List<Album> Albums
        {
            get
            {
                lock (Instance._albums)
                {
                    return Instance._albums;
                }
            }
        }

        public static List<Artist> Artists
        {
            get
            {
                lock (Instance._artists)
                {
                    return Instance._artists;
                }
            }
        }

        public static event OnTracksActualized OnTracksActualized;
        private static void OnTracksLoaded()
        {
            Instance._tracks = Instance._workingtracks;
            Instance._albums = Instance._workingalbums;
            Instance._artists = Instance._workingartists;
            Instance._workingtracks = null;
            Instance._workingartists = null;
            Instance._workingalbums = null;
            Stream audioLibraryStream = null;
            using (audioLibraryStream = new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate))
                new XmlSerializer(Instance.GetType()).Serialize(audioLibraryStream, Instance);
            OnTracksActualized?.Invoke();
        }

        #endregion


        #region Singleton Creation

        private static Library CreateInstance()
        {
            Library instance;
            Stream audioLibraryStream;
            using (audioLibraryStream = new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate))
                instance = (Library)
                new XmlSerializer(typeof(Library)).Deserialize(audioLibraryStream);
            instance._tracks = new List<Track>();
            foreach (var trackSerializer in instance.useForDeserializer)
            {
                instance._tracks.Add(trackSerializer.Track);
                var album = instance._albums.Find(salbum => trackSerializer.AlbumName == salbum.Name);
                if (album != null)
                {
                    album.Tracks.Add(trackSerializer.Track);
                    trackSerializer.Track.Album = album;
                    foreach (
                        var artist in
                            instance._artists.Where(artist => trackSerializer.ArtistsNames.Contains(artist.Name))
                                .ToList())
                    {
                        artist.Albums.Add(album);
                        album.Artists.Add(artist);
                        trackSerializer.Track.Artist.Add(artist);
                    }
                }
                else
                    foreach (
                        var artist in
                            instance._artists.Where(artist => trackSerializer.ArtistsNames.Contains(artist.Name))
                                .ToList())
                    {
                        artist.SingleTracks.Add(trackSerializer.Track);
                        trackSerializer.Track.Artist.Add(artist);
                    }
            }
            return instance;
        }

        private static readonly Library Instance = CreateInstance();

        private Library()
        {
        }

        internal static void Initialize()
        {
            new Thread(new ThreadStart(delegate
            {
                PathLibrary.Synchronize(new Dictionary<string, Action<List<string>, string>>
                {
                    {"*.mp3", Instance.OnFoundFile}
                });
                OnTracksLoaded();
            })).Start();
         }

        private BitmapImage CreateCover(File metaData)
        {
            if (metaData.Tag.IsEmpty || metaData.Tag.Pictures.Length == 0)
                return null;

            var picture = metaData.Tag.Pictures[0];
            MemoryStream mstream = new MemoryStream(picture.Data.Data);
            mstream.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmap = null;
            lock (this)
            {
                try
                {
                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = mstream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }
                catch (Exception e)
                {
                    bitmap = null;
                }
            }
            return bitmap;
        }

        private void OnFoundFile(List<string> path, string file)
        {
            File metaData = null;
            try
            {
                metaData = File.Create(file);
            }
            catch (Exception e)
            {
                return;
            }
            Album album = null;
            if (!metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Album))
            {
                album = _workingalbums.Find(searchedAlbum => searchedAlbum.Name == metaData.Tag.Album);
                if (album != null && album.Cover == null && metaData.Tag.Pictures.Length > 0)
                    lock (album)
                    {
                        album.Cover = CreateCover(metaData);
                    }
                if (album == null)
                    lock (_workingalbums)
                    {
                        _workingalbums.Add(album = new Album
                        {
                            Name = metaData.Tag.Album,
                            Cover = CreateCover(metaData)
                        });
                    }
            }
            List<Artist> artists = new List<Artist>();
            if (!metaData.Tag.IsEmpty)
            {
                foreach (var performer in metaData.Tag.Performers)
                {
                    var artist = _workingartists.Find(searchedArtist => searchedArtist.Name == performer);
                    if (artist == null)
                        lock (_workingartists)
                            _workingartists.Add(artist = new Artist
                            {
                                Name = performer
                            });
                    artists.Add(artist);
                }
            }
            TrackUserTag userTag = null;
            Track track = new Track
            {
                Album = album,
                Artist = artists,
                Name =
                    !metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Title)
                        ? metaData.Tag.Title
                        : Path.GetFileNameWithoutExtension(file),
                Duration = metaData.Properties.Duration,
                UserTag = new TrackUserTag(),
                RelativePaths = path,
                Path = file
            };
            lock (_workingartists)
                _workingtracks.Add(track);
            if (album != null)
                lock (album)
                {
                    album.Tracks.Add(track);
                    foreach (var artist in artists)
                    {
                        album.Artists.Add(artist);
                        lock (artist)
                            artist.Albums.Add(album);
                    }
                }
            foreach (var artist in artists)
                lock (artist)
                    artist.SingleTracks.Add(track);
        }

        #endregion

        #region Query

        private void InstanciedQuery()
        {
        }

        public static void Query()
        {
            Instance.InstanciedQuery();
        }

        public static Track SingleQueryOnTrack(Predicate<Track> predicate)
        {
            return Instance._tracks.Find(predicate);
        }

        public static Album SingleQueryOnAlbum(Predicate<Album> predicate)
        {
            return Instance._albums.Find(predicate);
        }
        #endregion
    }

    [Export(typeof (IStaticRessource))]
    public class AudioLibraryInstantiator : IStaticRessource
    {
        public AudioLibraryInstantiator()
        {
            Library.Initialize();
        }

        public void Initialize()
        {
        }
    }
}