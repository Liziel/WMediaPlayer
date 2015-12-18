using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio.Library;

namespace PlaylistPlugin.Models
{
    [Serializable]
    public class Playlist
    {
        #region Member Class

        [Serializable]
        public class Member
        {
            #region Properties

            private ITrack _track;
            private long _position;

            #endregion

            #region Public Accessors

            [XmlIgnore]
            public ITrack Track => _track;

            [XmlIgnore]
            public long Position => _position;

            #endregion

            #region Constructor

            public Member(ITrack track, long position)
            {
                _track = track;
                _position = position;
            }

            public Member()
            {
                _track = null;
                _position = 0;
            }

            #endregion

            #region Serializable Accessors

            [XmlElement("Position")]
            public long SerializePosition
            {
                get { return _position; }
                set { _position = value; }
            }

            [XmlElement("Track")]
            public string TrackSerial
            {
                get { return _track.MediaLibraryKey; }
                set
                {
                    _track =
                        (ITrack) Library.SingleQueryOnTrack(
                            track => track.MediaLibraryKey == value) ??
                        MediaPropertiesLibrary.Video.Library.Library.SingleQueryOnTrack(
                            track => track.MediaLibraryKey == value);
                }
            }

            #endregion
        }

        #endregion

        #region Playlist properties

        private string _name = "";
        private List<Member> _tracks;
        private TimeSpan _duration = TimeSpan.Zero;

        [XmlIgnore]
        public List<Member> Tracks => _tracks ?? (_tracks = new List<Member>());

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

        [XmlIgnore]
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
        public List<Member> TracksSerial
        {
            get { return _tracks; }
            set { _tracks = value; }
        }

        #endregion
    }
}