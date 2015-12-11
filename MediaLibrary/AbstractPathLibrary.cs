using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MediaLibrary
{
    internal abstract class AbstractPathLibrary
    {
        #region Save Locations

        public static string DataFolderLocation
        {
            get
            {
                string dataFolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                            "/GJVMediaPlayer";
                if (!Directory.Exists(dataFolderLocation)) Directory.CreateDirectory(dataFolderLocation);
                return dataFolderLocation;
            }
        }

        protected static string LibrariesLocation
        {
            get
            {
                string librariesLocation = DataFolderLocation + "/LibrariesData";
                if (!Directory.Exists(librariesLocation)) Directory.CreateDirectory(librariesLocation);
                return librariesLocation;
            }
        }

        #endregion

        #region Paths

        protected abstract List<string> Paths { get; set; }

        #endregion

        #region Load Save Synchronize

        protected void BaseSave(FileStream libraryConfigFile)
            => new XmlSerializer(GetType()).Serialize(libraryConfigFile, Paths);

        protected void BaseLoad(FileStream libraryConfigFile)
            => Paths = (List<string>) new XmlSerializer(typeof (List<string>)).Deserialize(libraryConfigFile);

        private static void EnumerateDirectory()
        {
        }

        protected void BaseSynchronize(Dictionary<string, Action<List<string>, string>> onSynchronizedFile)
        {
            foreach (var action in onSynchronizedFile)
            {
                foreach (string path in Paths)
                {
                    var localPath = new DirectoryInfo(path).FullName;
                    foreach (string filePath in Directory.GetFiles(path, action.Key, SearchOption.AllDirectories).AsEnumerable())
                    {
                        var fileDirectoryPath = new FileInfo(filePath).Directory?.FullName;
                        onSynchronizedFile[action.Key](fileDirectoryPath?.Substring(localPath.Length)
                            .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList(), filePath);
                    }
                }
            }
        }

        #endregion
    }
}