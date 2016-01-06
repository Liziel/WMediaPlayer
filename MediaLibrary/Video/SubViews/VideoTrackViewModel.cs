using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Video;
using static DispatcherLibrary.Dispatcher;
using WPFUiLibrary.UserControls.MediaTemplates.Models;

namespace MediaLibrary.Video.SubViews
{
    public class VideoTrackViewModel : Listener, INotifyPropertyChanged
    {
        private readonly CollectionViewSource _trackCollectionView = new CollectionViewSource();
        public ListCollectionView TracksView => _trackCollectionView.View as ListCollectionView;

        #region Track Access and Constructor

        public VideoTrackViewModel()
        {
            _trackCollectionView.Source = MediaPropertiesLibrary.Video.Library.Library.Videos;
            TracksView.Refresh();
            OnPropertyChanged(nameof(TracksView));

            PlayVideoTrack = delegate(Track track)
            {
                Dispatch("Multiple Track Selected For Play",
                    TracksView.Cast<TrackDefinition>(),
                    TracksView.Cast<TrackDefinition>().ToList().FindIndex(o => o == track));
            };
        }

        public PlayVideoTrack PlayVideoTrack { get; }

        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}