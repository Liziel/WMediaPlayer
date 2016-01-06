using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio.Library;

namespace PlaylistPlugin.Models
{
    internal delegate void PlaylistStateChanged(Playlist sender, PlaylistState state);

    public enum PlaylistState
    {
        Playing, InPlace, Stopped
    }

    [Serializable]
    public class Playlist
    {
        #region Member Class

        [Serializable]
        public class Member
        {
            #region Properties

            [XmlIgnore]
            public TrackDefinition Track { get; private set; }

            [XmlIgnore]
            public long Position { get; private set; }

            #endregion

            #region Constructor

            public Member(TrackDefinition track, long position)
            {
                Track = track;
                Position = position;
            }

            public Member()
            {
                Track = null;
                Position = 0;
            }

            #endregion

            #region Serializable Accessors

            [XmlElement("Position")]
            public long SerializePosition
            {
                get { return Position; }
                set { Position = value; }
            }

            [XmlElement("Track")]
            public string TrackSerial
            {
                get { return Track.MediaLibraryKey; }
                set
                {
                    Track =
                        (TrackDefinition) Library.Songs.FirstOrDefault(
                            track => track.MediaLibraryKey == value) ??
                        MediaPropertiesLibrary.Video.Library.Library.Videos.First(
                            track => track.MediaLibraryKey == value);
                }
            }

            #endregion
        }

        #endregion

        #region Playlist properties

        private string _name = "";
        private ObservableCollection<Member> _tracks;
        private TimeSpan _duration = TimeSpan.Zero;
        private PlaylistState _playlistState = PlaylistState.Stopped;

        [XmlIgnore]
        public ObservableCollection<Member> Tracks => _tracks ?? (_tracks = new ObservableCollection<Member>());

        [XmlIgnore]
        public TimeSpan Duration
        {
            get
            {
                return _duration != TimeSpan.Zero
                    ? _duration
                    : _duration = new TimeSpan(Tracks.Sum(member => member.Track.Duration.Ticks));
            }
        }

        [XmlElement("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Serializable Properties

        [XmlElement("Duration")]
        public long DurationSerial
        {
            get { return _duration.Ticks; }
            set { _duration = new TimeSpan(value); }
        }


        [XmlElement("Tracks")]
        public ObservableCollection<Member> TracksSerial
        {
            get { return _tracks; }
            set { _tracks = value; }
        }


        [XmlIgnore]
        internal PlaylistState PlaylistState
        {
            get { return _playlistState; }
            set
            {
                _playlistState = value;
                StateChanged(this, value);
            }
        }

        internal static event PlaylistStateChanged PlaylistStateChanged;

        private static void StateChanged(Playlist sender, PlaylistState state)
        {
            PlaylistStateChanged?.Invoke(sender, state);
        }

        #endregion

        public void AddTrack(TrackDefinition trackDefinition)
        {
            Tracks.Add(new Member(trackDefinition, Tracks.Count + 1));
        }
    }
}