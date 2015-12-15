using System;
using System.Collections.Generic;
using System.IO;

namespace MediaLibrary.Video.Library
{

    internal sealed class PathLibrary : AbstractPathLibrary
    {
        #region Location

        private static string AudioLibraryLocation => LibrariesLocation + "/videoLibrary.xml";

        #endregion

        #region Singleton

        private static readonly PathLibrary _instance = new PathLibrary();
        private PathLibrary()
        {
            try
            {
                using (var stream = new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate))
                    BaseLoad(stream);
            }
            catch (InvalidOperationException)
            {
                _paths = new List<string> { Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) };
            }

            using (var stream = new FileStream(AudioLibraryLocation, FileMode.OpenOrCreate))
                BaseSave(stream);
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