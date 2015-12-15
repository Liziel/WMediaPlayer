using System.Collections.Generic;

namespace MediaPropertiesLibrary.Audio
{
    public class Artist
    {
        public string Name { get; set; }

        public HashSet<Album> Albums { get; } = new HashSet<Album>();
        public List<Track> SingleTracks { get; } = new List<Track>();
    }
}