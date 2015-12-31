using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using MediaLibrary.Annotations;
using MediaLibrary.UserControlTemplates.Models;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.Audio.Pages.ArtistViewPanels
{
    public sealed class ArtistSinglesViewModel : INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private readonly CollectionViewSource _singles = new CollectionViewSource();
        public ListCollectionView Singles => _singles.View as ListCollectionView;

        public PlayAudioTrack PlayAudioTrack { get; }

        public ArtistSinglesViewModel(Artist artist)
        {
            PlayAudioTrack = delegate(Track track)
            {
                var l =
                    MediaPropertiesLibrary.Audio.Library.Library.Tracks.Where(t => t.Artists.Contains(artist))
                        .ToList();
                DispatcherLibrary.Dispatcher.Dispatch("Multiple Track Selected For Play", l, l.FindIndex(t => t == track));
            };
            _singles.Source = new ObservableCollection<Track>(artist.Singles);
            Singles.Refresh();
            OnPropertyChanged(nameof(Singles));
        }
    }
}