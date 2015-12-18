using System.ComponentModel;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using PlaylistPlugin.Models;

namespace PlaylistPlugin.ChildsViews
{
    public class CurrentPlaylistViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private Playlist _current;
        public Playlist Current
        {
            get { return _current; }
            private set
            {
                _current = value;
                OnPropertyChanged(nameof(Current));
            }
        }

    }
}