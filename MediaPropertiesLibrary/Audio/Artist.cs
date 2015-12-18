using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Audio
{
    public class Artist
    {
        public string Name { get; set; }

        [XmlIgnore]
        public HashSet<Album> Albums { get; } = new HashSet<Album>();
        [XmlIgnore]
        public List<Track> SingleTracks { get; } = new List<Track>();
    }
}