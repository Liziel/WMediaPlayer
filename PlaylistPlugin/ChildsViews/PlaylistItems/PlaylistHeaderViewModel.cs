using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PlaylistPlugin.Annotations;
using PlaylistPlugin.Models;
using WPFUiLibrary.Utils;

namespace PlaylistPlugin.ChildsViews.PlaylistItems
{


    public class PlaylistHeaderViewModel : INotifyPropertyChanged
    {
        public Playlist Playlist { get; private set; }

        private PlaylistState _playlistState = PlaylistState.Stopped;
        public PlaylistState PlaylistState
        {
            get { return _playlistState; }
            set {
                _playlistState = value;
                OnPropertyChanged(nameof(PlaylistState)); }
        }

        public PlaylistHeaderViewModel()
            : this(new Playlist {Name = "Debugger" })
        { }

        public PlaylistHeaderViewModel(Playlist playlist)
        {
            Playlist = playlist;
            Play = new UiCommand(o => DispatcherLibrary.Dispatcher.Dispatch("Playlist Plugin: Set Playlist", playlist));
            Playlist.PlaylistStateChanged += (sender, state) =>
            {
                if (sender == Playlist)
                {
                    PlaylistState = state;
                }
            };
        }

        public ICommand Play { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}