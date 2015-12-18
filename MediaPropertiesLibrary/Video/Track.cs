using System;
using System.Collections.Generic;

namespace MediaPropertiesLibrary.Video
{
    public class Track : ITrack
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<string> RelativePath{ get; set; }

        public TimeSpan Duration { get; set; }
        public string StringifiedDuration
            => Duration > TimeSpan.FromHours(1) ? Duration.ToString(@"hh\:mm\:ss") : Duration.ToString(@"mm\:ss");

        public Serie Serie { get; set; }
        public string SerieName => Serie?.Name;

        public UserTrackDefinition UserTrackDefinition { get; set; }

        #region Herited From ITracks

        public string MediaLibraryKey => Path;

        #endregion
    }

    public class UserTrackDefinition
    {
        public string SerieName { get; set; }
        public string Name { get; set; }
    }
}