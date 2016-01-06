using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Serialization;
using PluginLibrary;
using File = TagLib.File;

namespace MediaPropertiesLibrary.Audio.Library
{
    [Serializable]
    public class TrackSerializer
    {
        public Track Track { get; set; }
        public string AlbumName { get; set; }
        public List<string> ArtistsNames { get; set; }
    }

    public class Library
    {
        #region Serialization

        private static string AudioLibraryLocation => Locations.Libraries + "/SavedAudioLibrary.xml";

        private List<TrackSerializer> _useForTrackDeserializer;

        [XmlArray("Songs", Order = 2)]
        public List<TrackSerializer> SerializableTracks
        {
            get
            {
                if (_tracks == null)
                    return _useForTrackDeserializer = new List<TrackSerializer>();
                return _tracks.Select(track => new TrackSerializer
                {
                    Track = track,
                    AlbumName = track.Album?.Name,
                    ArtistsNames = track.Artists.Select(artist => artist.Name).ToList()
                }).ToList();
            }
        }

        [XmlArray("Albums", Order = 1)]
        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public ObservableCollection<Album> SerializableAlbums => _albums;

        [XmlArray("Artists", Order = 0)]
        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public ObservableCollection<Artist> SerializableArtists => _artists;

        #endregion

        #region Library Songs Artists & Albums

        private ObservableCollection<Track>             _tracks = null;
        private readonly ObservableCollection<Album>    _albums = new ObservableCollection<Album>();
        private readonly ObservableCollection<Artist>   _artists = new ObservableCollection<Artist>();

        public static ObservableCollection<Track> Songs
        {
            get
            {
                lock (Instance._tracks)
                {
                    return Instance._tracks;
                }
            }
        }

        public static ObservableCollection<Album> Albums
        {
            get
            {
                lock (Instance._albums)
                {
                    return Instance._albums;
                }
            }
        }

        public static ObservableCollection<Artist> Artists
        {
            get
            {
                lock (Instance._artists)
                {
                    return Instance._artists;
                }
            }
        }

        private static void OnTracksLoaded()
        {

            Application.Current.Dispatcher.Invoke(
            delegate
            {
                foreach (var track in Instance._workingTracks)
                    Instance._tracks.Add(track);
                foreach (var album in Instance._workingAlbums)
                    Instance._albums.Add(album);
                foreach (var artist in Instance._workingArtists)
                    Instance._artists.Add(artist);
                Stream audioLibraryStream = null;
                using (audioLibraryStream = new FileStream(AudioLibraryLocation, FileMode.Truncate))
                    new XmlSerializer(Instance.GetType()).Serialize(audioLibraryStream, Instance);
                Instance._working = false;
            }, DispatcherPriority.DataBind);
        }

        #endregion


        #region Singleton Creation

        private static Library CreateInstance()
        {
            Library instance;
            try
            {
                using (var stream = new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate))
                    instance = (Library)
                        new XmlSerializer(typeof(Library)).Deserialize(stream);
            }
            catch (Exception)
            {
                using (var stream = new FileStream(AudioLibraryLocation, FileMode.Open))
                    instance = (Library)
                        new XmlSerializer(typeof(Library)).Deserialize(stream);
            }
            instance._tracks = new ObservableCollection<Track>();
            foreach (var trackSerializer in instance._useForTrackDeserializer.Where(trackSerializer => System.IO.File.Exists(trackSerializer.Track.Path)))
            {
                var album = instance._albums.FirstOrDefault(salbum => trackSerializer.AlbumName == salbum.Name);
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
                        trackSerializer.Track.Artists.Add(artist);
                    }
                }
                else
                    foreach (
                        var artist in
                            instance._artists.Where(artist => trackSerializer.ArtistsNames.Contains(artist.Name))
                                .ToList())
                    {
                        artist.Singles.Add(trackSerializer.Track);
                        trackSerializer.Track.Artists.Add(artist);
                    }
                instance._tracks.Add(trackSerializer.Track);
            }
            foreach (var album in instance._albums.Where(album => album.Tracks.Count == 0).ToList())
                instance._albums.Remove(album);
            foreach (
                var artist in instance._artists.Where(artist => artist.Singles.Count == 0 && artist.Albums.Count == 0).ToList())
                instance._artists.Remove(artist);
            return instance;
        }

        private static readonly Library Instance = CreateInstance();

        private Library()
        {
        }

        private readonly List<Track> _workingTracks = new List<Track>();
        private readonly List<Album> _workingAlbums = new List<Album>();
        private readonly List<Artist> _workingArtists = new List<Artist>();
        private bool _working = true;
        internal static void Initialize()
        {
            new Thread(new ThreadStart(delegate
            {
                PathLibrary.Synchronize(new Dictionary<string, Action<List<string>, string>>
                {
                    {"*.mp3", Instance.OnFoundFile},
                    {"*.wma", Instance.OnFoundFile}
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

            BitmapImage bitmap;
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
                album = _workingAlbums.FirstOrDefault(searchedAlbum => searchedAlbum.Name == metaData.Tag.Album) ?? _albums.FirstOrDefault(searchedAlbum => searchedAlbum.Name == metaData.Tag.Album);
                if (album != null && album.Cover == null && metaData.Tag.Pictures.Length > 0)
                    lock (album)
                    {
                        album.Cover = CreateCover(metaData);
                    }
                if (album == null)
                    lock (_albums)
                    {
                        _workingAlbums.Add(album = new Album
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
                    var artist = _workingArtists.FirstOrDefault(searchedArtist => searchedArtist.Name == performer) ?? _artists.FirstOrDefault(searchedArtist => searchedArtist.Name == performer);
                    if (artist == null)
                        lock (_artists)
                            _workingArtists.Add(artist = new Artist
                            {
                                Name = performer
                            });
                    artists.Add(artist);
                }
            }
            {
                if ((_workingTracks.FirstOrDefault(track => track.Path == file) ?? _tracks.FirstOrDefault(track => track.Path == file)) != null) return;
            }
            {
                TrackUserTag userTag = null;
                Track track =  new Track
                {
                    Album = album,
                    Artists = artists,
                    Name =
                        !metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Title)
                            ? metaData.Tag.Title
                            : Path.GetFileNameWithoutExtension(file),
                    Duration = metaData.Properties.Duration,
                    UserTag = new TrackUserTag(),
                    Genres = new List<string>(metaData.Tag.Genres),
                    RelativePaths = path,
                    Path = file
                };
                _workingTracks.Add(track);
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
                        artist.Singles.Add(track);
            }
        }

        #endregion

        public static void Save()
        {
            if (Instance._working) return;
            lock (Instance)
                using (var audioLibraryStream = new FileStream(AudioLibraryLocation, FileMode.Truncate))
                    new XmlSerializer(Instance.GetType()).Serialize(audioLibraryStream, Instance);
        }
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