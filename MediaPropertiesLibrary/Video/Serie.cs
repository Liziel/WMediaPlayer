using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using MediaPropertiesLibrary.Annotations;

namespace MediaPropertiesLibrary.Video
{
    public class Serie : INotifyPropertyChanged
    {
        private MediaState _state;

        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
        public BitmapImage Cover { get; set; }

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

    public class UserSerieDefinition
    {
        public string Name { get; set; }
        public string CoverPath { get; set; }
    }
}