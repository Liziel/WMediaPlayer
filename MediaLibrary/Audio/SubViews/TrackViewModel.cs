using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaLibrary.UserControlTemplates.Models;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace MediaLibrary.Audio.SubViews
{
    public sealed class TrackViewModel : Listener, INotifyPropertyChanged, IComparer
    {
        #region Sorting and Searching

        private enum OrderBy
        {
            Track, Album, Artist, Time
        }

        private bool _orderInversion = false;
        private static OrderBy _orderBy = OrderBy.Track;

        public UiCommand OrderByTrack { get; }
        public UiCommand OrderByAlbum { get; }
        public UiCommand OrderByArtist { get; }
        public UiCommand OrderByTime { get; }

        public int Compare(object rhs, object lhs)
        {
            var x = rhs as MediaPropertiesLibrary.Audio.Track;
            var y = lhs as MediaPropertiesLibrary.Audio.Track;
            int result = 0;

            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            switch (_orderBy)
            {
                case OrderBy.Album:
                    result = string.Compare(x.Album?.Name, y.Album?.Name, StringComparison.OrdinalIgnoreCase);
                    break;
                case OrderBy.Artist:
                    var xN = x.Artists.Count > 0 ? x.Artists[0].Name : "";
                    var yN = y.Artists.Count > 0 ? y.Artists[0].Name : "";
                    result = string.Compare(xN, yN, StringComparison.OrdinalIgnoreCase);
                    break;
                case OrderBy.Time:
                    result = TimeSpan.Compare(x.Duration, y.Duration);
                    break;
                case OrderBy.Track:
                    result = string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
                    break;
            }
            return _orderInversion ? -result : result;
        }

        private readonly CollectionViewSource _trackCollectionView = new CollectionViewSource();
        public ListCollectionView TracksView => _trackCollectionView.View as ListCollectionView;

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                TracksView?.Refresh();
            }
        }

        private void OrderByAffectation(OrderBy order)
        {
            _orderInversion = _orderBy == order ? !_orderInversion : _orderInversion;
            _orderBy = order;
            TracksView?.Refresh();
        }

        #endregion

        #region Track Access and Constructor

        public TrackViewModel()
        {
            _trackCollectionView.Source = MediaPropertiesLibrary.Audio.Library.Library.Tracks;
            TracksView.Filter += (item) => ((MediaPropertiesLibrary.Audio.Track)item).Name.ToLower().Contains(SearchText.ToLower());
            TracksView.CustomSort = this;
            TracksView.Refresh();
            OnPropertyChanged(nameof(TracksView));

            PlayAudioTrack = track => Dispatch("Multiple Track Selected For Play",
                TracksView.Cast<TrackDefinition>(),
                TracksView.Cast<TrackDefinition>().ToList().FindIndex(o => o == track));

            OrderByTime = new UiCommand(o => OrderByAffectation(OrderBy.Time));
            OrderByArtist = new UiCommand(o => OrderByAffectation(OrderBy.Artist));
            OrderByAlbum = new UiCommand(o => OrderByAffectation(OrderBy.Album));
            OrderByTrack = new UiCommand(o => OrderByAffectation(OrderBy.Track));
        }

        #endregion

        public PlayAudioTrack PlayAudioTrack { get; }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UiCommand ShowArtist { get; } = new UiCommand(track => Dispatch("AudioLibrary: View Artist", ((MediaPropertiesLibrary.Audio.Track)track).Artists));
        public UiCommand ShowAlbum { get; } = new UiCommand(track => Dispatch("AudioLibrary: View Album", ((MediaPropertiesLibrary.Audio.Track)track).Album));

        #endregion
    }
}