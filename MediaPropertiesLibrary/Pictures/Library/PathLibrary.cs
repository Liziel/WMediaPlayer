using System;
using System.Collections.Generic;
using System.IO;

namespace MediaPropertiesLibrary.Pictures.Library
{
    internal sealed class PathLibrary : AbstractPathLibrary
    {
        #region Location

        private static string AudioLibraryLocation => LibrariesLocation + "/picturesLibrary.xml";

        #endregion

        #region Singleton

        private static readonly PathLibrary _instance = new PathLibrary();
        private PathLibrary()
        { BaseLoad(new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate)); }

        #endregion

        #region Xml Load / Save

        private List<string> _paths; 
        protected override List<string> Paths { get { return _paths; } set { _paths = value; } }

        public static void Save()
        {
            _instance.BaseSave(new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate));
        }

        public static void Synchronize(Dictionary<string, Action<List<string>, string>> onSynchronizedFile)
        {
            _instance.BaseSynchronize(onSynchronizedFile);
        }

        #endregion

    }
}