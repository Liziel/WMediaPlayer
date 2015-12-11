using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media.Imaging;
using File = TagLib.File;

namespace MediaLibrary.Audio
{
    internal sealed class Album
    {
        public  string          Name;
        public  BitmapImage     Cover;

        public  HashSet<Artist> Artists { get; } = new HashSet<Artist>();
        public  List<Track>     Tracks { get; } = new List<Track>();
    }

    internal class Track
    {
        public string   Name;
        public string   Path;
        public TimeSpan Duration;

        public Album           Album;
        public List<Artist>    Artist;

        public List<string> RelativePaths;
    }

    internal class Artist
    {
        public string       Name;

        public HashSet<Album>   Albums { get; } = new HashSet<Album>(); 
        public List<Track>      SingleTracks { get; } = new List<Track>();
    }

    public class Library
    {
        private List<Track>     Tracks;
        private List<Album>     Albums;
        private List<Artist>    Artists;

        #region Singleton Creation

        private static Library _instance = new Library();

        private Library()
        {
            new Thread(new ThreadStart(delegate
            {
                PathLibrary.Synchronize(new Dictionary<string, Action<List<string>, string>>
                {
                    {"*.mp3", OnFoundFile}
                });
            })).Start();
        }

        private BitmapImage CreateCover(File metaData)
        {
            if (metaData.Tag.IsEmpty || metaData.Tag.Pictures.Length == 0)
                return null;

            var picture = metaData.Tag.Pictures[0];
            MemoryStream mstream = new MemoryStream(picture.Data.Data);
            mstream.Seek(0, SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = mstream;
            bitmap.EndInit();
            return bitmap;
        }

        private void OnFoundFile(List<string> path, string file)
        {
            File metaData = File.Create(file);
            Album album = null;
            if (!metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Album))
            {
                album = Albums.Find(searchedAlbum => searchedAlbum.Name == metaData.Tag.Album);
                if (album != null && album.Cover == null && metaData.Tag.Pictures.Length > 0)
                    lock (album)
                    {
                        album.Cover = CreateCover(metaData);
                    }
                if (album == null)
                    lock (Albums)
                    {
                        Albums.Add(album = new Album
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
                    var artist = Artists.Find(searchedArtist => searchedArtist.Name == performer);
                    if (artist == null)
                        lock (Artists)
                        {
                            Artists.Add(artist = new Artist
                            {
                                Name = performer
                            });
                        }
                    artists.Add(artist);
                }
            }
            Track track = new Track
            {
                Album = album,
                Artist = artists,

                Name =
                    !metaData.Tag.IsEmpty && !string.IsNullOrEmpty(metaData.Tag.Title)
                        ? metaData.Tag.Title
                        : Path.GetFileNameWithoutExtension(file),
                Duration = metaData.Properties.Duration,

                RelativePaths = path,
                Path = file
            };
            lock (Artists)
            {
                Tracks.Add(track);
            }
            if (album != null)
                lock (album)
                {
                    album.Tracks.Add(track);
                    foreach (var artist in artists)
                    {
                        album.Artists.Add(artist);
                        lock (artist)
                        {
                            artist.Albums.Add(album);
                        }
                    }
                }
            foreach (var artist in artists)
            {
                lock (artist)
                {
                    artist.SingleTracks.Add(track);
                }
            }
        }

        #endregion

        private void InstanciedQuery()
        {
            
        }
        public static void Query()
        {
            _instance.InstanciedQuery();
        }
    }
}