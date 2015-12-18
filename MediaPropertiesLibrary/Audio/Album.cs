using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Audio
{
    public class Album
    {
        public string Name { get; set; }

        [XmlIgnore]
        public BitmapImage Cover { get; set; }
        [XmlIgnore]
        public HashSet<Artist>  Artists { get; } = new HashSet<Artist>();
        [XmlIgnore]
        public string           FirstArtistName => Artists.First()?.Name;
        [XmlIgnore]
        public List<Track> Tracks { get; } = new List<Track>();
    }
}