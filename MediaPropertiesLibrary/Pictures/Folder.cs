using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MediaPropertiesLibrary.Pictures
{
    public class Serial
    {
        public ObservableCollection<Picture> Pictures { get; set; }
        public ObservableCollection<Folder> Folders { get; set; } 
    }

    public class Folder
    {
        [XmlElement("Name")]
        public string Name { get; set; } 

        [XmlIgnore]
        public ObservableCollection<Picture> Pictures { get; } = new ObservableCollection<Picture>();
        [XmlIgnore]
        public ObservableCollection<Folder> Folders { get; } = new ObservableCollection<Folder>();
        [XmlIgnore]
        public Folder Parent { get; set; }

        #region Serialization

        [XmlElement("Serial")]
        public Serial Serial
        {
            get
            {
                return new Serial
                {
                    Folders = Folders,
                    Pictures = Pictures
                };
            }
            set
            {
                foreach (var folder in value.Folders.Where(folder => folder.Pictures.Count > 0 || folder.Folders.Count > 0))
                {
                    folder.Parent = this;
                    Folders.Add(folder);
                }
                foreach (var picture in value.Pictures.Where(picture => File.Exists(picture.Path)).OrderBy(picture => picture.Path))
                {
                    picture.Parent = this;
                    Pictures.Add(picture);
                }
            }
        }

        #endregion
    }
}