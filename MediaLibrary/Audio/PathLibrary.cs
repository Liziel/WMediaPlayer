using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MediaLibrary.Audio
{
    internal sealed class PathLibrary : AbstractPathLibrary
    {
        #region Location

        private static string AudioLibraryLocation => LibrariesLocation + "/audioLibrary.xml";

        #endregion

        #region Singleton

        private static readonly PathLibrary _instance = new PathLibrary();
        private PathLibrary()
        {
        }

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