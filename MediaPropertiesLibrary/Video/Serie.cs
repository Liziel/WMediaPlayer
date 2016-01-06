using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using MediaPropertiesLibrary.Annotations;

namespace MediaPropertiesLibrary.Video
{
    public class Serie : INotifyPropertyChanged
    {
        private MediaState _state;

        public string Name { get; set; }
        public string Cover { get; set; }

        [XmlIgnore]
        public List<Track>  Tracks { get; set; }
        [XmlIgnore]
        public MediaState State { get {return _state;} set { _state = value; OnPropertyChanged(nameof(State)); } }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}