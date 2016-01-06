using System;
using System.Collections.Generic;
using System.IO;

namespace MediaPropertiesLibrary.Pictures.Library
{
    internal sealed class PathLibrary : AbstractPathLibrary
    {
        #region Location

        private static string PictureLibraryLocation => LibrariesLocation + "/picturesLibrary.xml";

        #endregion

        #region Singleton

        private static readonly PathLibrary _instance = new PathLibrary();

        private PathLibrary()
        {
            try
            {
                using (var stream = new FileStream(PictureLibraryLocation, FileMode.OpenOrCreate))
                    BaseLoad(stream);
            }
            catch (Exception)
            {
                _paths = new List<string> { Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) };
            }

            using (var stream = new FileStream(PictureLibraryLocation, FileMode.OpenOrCreate))
                BaseSave(stream);
        }

        #endregion

        #region Xml Load / Save

        private List<string> _paths; 
        protected override List<string> Paths { get { return _paths; } set { _paths = value; } }

        public static void Save()
        {
            using (var stream = new FileStream(PictureLibraryLocation, FileMode.OpenOrCreate))
                _instance.BaseSave(stream);
        }

        public static void Synchronize(Dictionary<string, Action<List<string>, string>> onSynchronizedFile)
        {
            _instance.BaseSynchronize(onSynchronizedFile);
        }

        #endregion

    }
}