using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Pictures
{
    public class SlideShowSerial
    {
        public List<string> SlideShows { get; set; }
    }

    public class Picture
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("Path")]
        public string Path { get; set; }

        [XmlIgnore]
        public Folder Parent { get; set; }
        [XmlIgnore]
        public List<SlideShow> SlideShows { get; } = new List<SlideShow>();

        [XmlElement("SlideShows")]
        public SlideShowSerial Serial {
            get
            {
                return new SlideShowSerial {SlideShows = SlideShows.Select(show => show.Name).ToList()};
            }
            set
            {
                foreach (var slideShowName in value.SlideShows)
                {
                    var slideshow = Library.Library.SlideShows.FirstOrDefault(show => show.Name == slideShowName);
                    if (slideshow == null)
                        Library.Library.SlideShows.Add(slideshow = new SlideShow());
                    slideshow.Pictures.Add(this);
                }
            }
        }
    }
}