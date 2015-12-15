using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace MediaPropertiesLibrary.Audio
{
    public class Album
    {
        public string Name { get; set; }
        public BitmapImage Cover { get; set; }

        public HashSet<Artist>  Artists { get; } = new HashSet<Artist>();
        public string           FirstArtistName => Artists.First()?.Name;
        public List<Track> Tracks { get; } = new List<Track>();
    }
}