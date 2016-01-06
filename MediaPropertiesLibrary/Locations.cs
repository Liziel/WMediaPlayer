using System;
using System.IO;

namespace MediaPropertiesLibrary
{
    public static class Locations
    {
        public static string DataFolder
        {
            get
            {
                string dataFolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                            "/GJVMediaPlayer";
                if (!Directory.Exists(dataFolderLocation)) Directory.CreateDirectory(dataFolderLocation);
                return dataFolderLocation;
            }
        }

        public static string Libraries
        {
            get
            {
                string librariesLocation = DataFolder + "/LibrariesData";
                if (!Directory.Exists(librariesLocation)) Directory.CreateDirectory(librariesLocation);
                return librariesLocation;
            }
        }
    }
}