using System;

namespace MediaPropertiesLibrary.Video
{
    public class Track
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public TimeSpan Duration { get; set; }
        public string StringifiedDuration
            => Duration > TimeSpan.FromHours(1) ? Duration.ToString(@"hh\:mm\:ss") : Duration.ToString(@"mm\:ss");

        public Serie Serie { get; set; }
        public string SerieName => Serie != null ? Serie.Name : "";
    }

    public class UserTrack
    {
        public string SerieName { get; set; }
        public string Name { get; set; }
    }
}