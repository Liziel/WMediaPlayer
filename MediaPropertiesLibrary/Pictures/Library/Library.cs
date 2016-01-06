using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PluginLibrary;

namespace MediaPropertiesLibrary.Pictures.Library
{
    public class Library
    {
        public static ObservableCollection<SlideShow> SlideShows { get; } = new ObservableCollection<SlideShow>();
        public static Folder Root => Instance.RootFolder;
        private static Library Instance { get; } = CreateInstance();

        [XmlElement("Root")]
        public Folder RootFolder { get; set; } = new Folder {Name = "My Pictures Folders"};

        private Library()
        {
            
        }

        private static Library CreateInstance()
        {
            Library instance;

            try
            {
                using (var stream = new FileStream(Locations.Libraries + "/SavedPictureLibrary.xml", FileMode.OpenOrCreate))
                    instance = (Library) new XmlSerializer(typeof(Library)).Deserialize(stream);
            }
            catch (Exception)
            {
                return new Library();
            }

            return instance;
        }

        private bool _working = true;
        internal static void Initialize()
        {
            new Thread(new ThreadStart(delegate
            {
                PathLibrary.Synchronize(new Dictionary<string, Action<List<string>, string>>
                {
                    {"*.jpg", Instance.OnFoundFile },
                    {"*.png", Instance.OnFoundFile },
                });
                Instance._working = false;
                Save();
            })).Start();
        }

        private void OnFoundFile(List<string> dividedPath, string path)
        {
            Folder folder = Root;
            foreach (var folderName in dividedPath)
            {
                var next = folder.Folders.FirstOrDefault(f => f.Name == folderName);

                if (next == null)
                {
                    next = new Folder { Name = folderName, Parent = folder };
                    folder.Folders.Add(next);
                }

                folder = next;
            }

            if (folder.Pictures.FirstOrDefault(picture => picture.Path == path) == null)
                folder.Pictures.Add(new Picture
                {
                    Name = Path.GetFileNameWithoutExtension(path),
                    Path = path,
                    Parent = folder
                });
        }

        public static void Save()
        {
            if (Instance._working) return;
            lock (Instance)
                try
                {
                    using (var stream = new FileStream(Locations.Libraries + "/SavedPictureLibrary.xml", FileMode.Truncate))
                        new XmlSerializer(Instance.GetType()).Serialize(stream, Instance);
                }
                catch (Exception)
                {
                    Instance._working = true;
                    Task.Delay(10).ContinueWith(t =>
                    {
                        Instance._working = false;
                        Save();
                    });
                }
        }
    }

    [Export(typeof (IStaticRessource))]
    public class AudioLibraryInstantiator : IStaticRessource
    {
        public AudioLibraryInstantiator()
        {
            Library.Initialize();
        }

        public void Initialize()
        {
        }
    }

}