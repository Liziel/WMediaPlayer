using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Pictures
{
    public class SlideShow
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public ObservableCollection<Picture> Pictures { get; } = new ObservableCollection<Picture>();
    }
}