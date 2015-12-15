using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MediaPropertiesLibrary.Video
{
    public class Serie
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
        public BitmapImage Cover { get; set; }
    }

    public class UserSerieDefinition
    {
        public string Name { get; set; }
        public string CoverPath { get; set; }
    }
}