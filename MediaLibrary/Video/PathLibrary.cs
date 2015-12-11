using System;
using System.Collections.Generic;
using System.IO;

namespace MediaLibrary.Video
{

    internal sealed class PathLibrary : AbstractPathLibrary
    {
        #region Location

        private static string AudioLibraryLocation => LibrariesLocation + "/videoLibrary.xml";

        #endregion

        #region Singleton

        private static readonly PathLibrary _instance = new PathLibrary();
        private PathLibrary(){}

        #endregion

        #region Xml Load / Save

        private List<string> _paths; 
        protected override List<string> Paths { get { return _paths; } set { _paths = value; } }

        public static void Save()
        {
            _instance.BaseSave(new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate));
        }

        private static void Synchronize(Dictionary<string, Action<List<string>, string>> onSynchronizedFile)
        {
            _instance.BaseSynchronize(onSynchronizedFile);
        }

        #endregion
    }
}