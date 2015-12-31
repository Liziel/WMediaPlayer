using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MediaPropertiesLibrary.Annotations;

namespace MediaPropertiesLibrary
{
    public enum MediaState
    {
        Stopped, End,
        Playing, Paused
    }

    public abstract class TrackDefinition : INotifyPropertyChanged
    {
        private MediaState _state = MediaState.Stopped;
        [XmlIgnore]
        public abstract TimeSpan     Duration { get; set; }
        [XmlIgnore]
        public abstract string       MediaLibraryKey { get; }

        [XmlIgnore]
        public MediaState State
        {
            get { return _state; }
            set
            {
                _state = value; 
                OnPropertyChanged(nameof(State));
                OnStateChanged(value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected abstract void OnStateChanged(MediaState state);
    }
}