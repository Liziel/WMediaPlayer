using System;

namespace MediaPropertiesLibrary
{
    public delegate void TrackPlayed();

    public delegate void TrackPaused();

    public delegate void TrackStopped();

    public interface ITrack
    {
        event TrackPaused TrackPaused;
        event TrackPlayed TrackPlayed;
        event TrackStopped TrackStopped;

        TimeSpan Duration { get; }
        string      MediaLibraryKey { get; }
    }
}