using System;

namespace MediaPropertiesLibrary
{
    public interface ITrack
    {
        TimeSpan    Duration { get; }
        string      MediaLibraryKey { get; }
    }
}