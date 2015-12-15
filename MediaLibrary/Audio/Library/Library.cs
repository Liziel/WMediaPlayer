using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaPropertiesLibrary.Audio;
using PluginLibrary;
using File = TagLib.File;

namespace MediaLibrary.Audio.Library
{ 

    public delegate void OnAllTracksLoaded();

    public class Library
    {
        #region Library Tracks Artists & Albums

        private readonly List<Track> _tracks = new List<Track>();
        private readonly List<Album> _albums = new List<Album>();
        private readonly List<Artist> _artists = new List<Artist>();

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

        private int _triggerTracksLoaded = 0;
        public static event OnAllTracksLoaded TracksLoaded;

        #endregion

        #region Library UserTags

        private static string AudioLibraryLocation => AbstractPathLibrary.LibrariesLocation + "/audioLibrary.xml";

        private readonly Dictionary<string, TrackUserTag> _userTags = new Dictionary<string, TrackUserTag>();
//            CreateUserTags(new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate));

        private static Dictionary<string, TrackUserTag> CreateUserTags(Stream libraryConfigFile) =>
            (Dictionary<string, TrackUserTag>)
                new XmlSerializer(typeof (Dictionary<string, TrackUserTag>)).Deserialize(libraryConfigFile);

        private void SaveUsertags(Stream libraryConfigFile) =>
            new XmlSerializer(_userTags.GetType()).Serialize(libraryConfigFile, _userTags);

        #endregion

        #region Singleton Creation

        private static readonly Library Instance = new Library();

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
                //Instance.SaveUsertags(new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate));
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
                album = _albums.Find(searchedAlbum => searchedAlbum.Name == metaData.Tag.Album);
                if (album != null && album.Cover == null && metaData.Tag.Pictures.Length > 0)
                    lock (album)
                    {
                        album.Cover = CreateCover(metaData);
                    }
                if (album == null)
                    lock (_albums)
                    {
                        _albums.Add(album = new Album
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
                    var artist = _artists.Find(searchedArtist => searchedArtist.Name == performer);
                    if (artist == null)
                        lock (_artists)
                            _artists.Add(artist = new Artist
                            {
                                Name = performer
                            });
                    artists.Add(artist);
                }
            }
            TrackUserTag userTag = null;
            _userTags.TryGetValue(file, out userTag);
            Track track = new Track
            {
                Album = album,
                Artist = artists,
                Name =
                    !metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Title)
                        ? metaData.Tag.Title
                        : Path.GetFileNameWithoutExtension(file),
                Duration = metaData.Properties.Duration,
                UserTag = userTag ?? (_userTags[file] = new TrackUserTag()),
                RelativePaths = path,
                Path = file
            };
            lock (_artists)
                _tracks.Add(track);
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

        #endregion

        private static void OnTracksLoaded()
        {
            TracksLoaded?.Invoke();
        }
    }

    [Export(typeof (IStaticRessource))]
    public class LibraryInstantiator : IStaticRessource
    {
        public LibraryInstantiator()
        {
            Library.Initialize();
        }

        public void Initialize()
        {
        }
    }
}