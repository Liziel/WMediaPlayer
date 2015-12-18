using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using PluginLibrary;
using File = TagLib.File;

namespace MediaPropertiesLibrary.Audio.Library
{ 

    public delegate void OnAllTracksLoaded();

    public class Library
    {
        private static string AudioLibraryLocation => AbstractPathLibrary.LibrariesLocation + "/SavedAudioLibrary.xml";

        [XmlElement("Tracks")]
        public List<Track> SerializableTracks { get { return _tracks; } set { _tracks = value; } }
        [XmlElement("Albums")]
        public List<Album> SerializableAlbums { get { return _albums; } set { _albums = value; } }
        [XmlElement("Artists")]
        public List<Artist> SerializableArtists { get { return _artists; } set { _artists = value; } }

        #region Library Tracks Artists & Albums

        private List<Track> _tracks = new List<Track>();
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

        public static event OnAllTracksLoaded TracksLoaded;
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
            TracksLoaded?.Invoke();
        }

        #endregion


        #region Singleton Creation

        private static Library CreateInstance()
        {
            Stream audioLibraryStream = null;
            using (audioLibraryStream = new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate))
                return (Library)
                new XmlSerializer(typeof(Library)).Deserialize(audioLibraryStream);
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
            return null;
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