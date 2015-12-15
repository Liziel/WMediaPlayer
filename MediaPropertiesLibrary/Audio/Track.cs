using System;
using System.Collections.Generic;

namespace MediaPropertiesLibrary.Audio
{
    public class Track
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public TimeSpan Duration { get; set; }

        public string StringifiedDuration
            => Duration > TimeSpan.FromHours(1) ? Duration.ToString(@"hh\:mm\:ss") : Duration.ToString(@"mm\:ss");

        public TrackUserTag UserTag { get; set; }

        public Album Album { get; set; }
        public string AlbumName => Album != null ? Album.Name : "";

        public List<Artist> Artist { get; set; }
        public string ArtistName => Artist.Count > 0 ? Artist[0].Name : "";

        public List<string> RelativePaths { get; set; }
    }

    public sealed class TrackUserTag
    {
    }
}