using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Audio;
using UiLibrary;
using UiLibrary.Utils;
using Dispatcher = DispatcherLibrary.Dispatcher;

namespace MediaLibrary.Audio.SubViews
{
    public sealed class AudioTrackViewModel : Listener, INotifyPropertyChanged, IComparer
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
                    var xN = x.Artist.Count > 0 ? x.Artist[0].Name : "";
                    var yN = y.Artist.Count > 0 ? y.Artist[0].Name : "";
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

        #endregion

        #region Track Access and Constructor

        private List<MediaPropertiesLibrary.Audio.Track> _tracksAccess = null;
        public List<MediaPropertiesLibrary.Audio.Track> TracksAccess => _tracksAccess;

        public AudioTrackViewModel()
        {
            _tracksAccess = MediaPropertiesLibrary.Audio.Library.Library.Tracks;
            _trackCollectionView.Source = _tracksAccess;
            TracksView.Filter += (item) => ((MediaPropertiesLibrary.Audio.Track)item).Name.ToLower().Contains(SearchText.ToLower());
            TracksView.CustomSort = this;
            TracksView.Refresh();
            OnPropertyChanged(nameof(TracksView));

            MediaPropertiesLibrary.Audio.Library.Library.OnTracksActualized += OnLibraryOnOnTracksActualized;
            PlayTrack = new UiCommand(o => Play(o as Track));

            OrderByTime = new UiCommand(o => OrderByAffectation(OrderBy.Time));
            OrderByArtist = new UiCommand(o => OrderByAffectation(OrderBy.Artist));
            OrderByAlbum = new UiCommand(o => OrderByAffectation(OrderBy.Album));
            OrderByTrack = new UiCommand(o => OrderByAffectation(OrderBy.Track));
        }

        private void OrderByAffectation(OrderBy order)
        {
            _orderInversion = _orderBy == order ? !_orderInversion : _orderInversion;
            _orderBy = order;
            TracksView?.Refresh();
        }

        private void OnLibraryOnOnTracksActualized()
        {
            _tracksAccess = MediaPropertiesLibrary.Audio.Library.Library.Tracks;
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    _trackCollectionView.Source = _tracksAccess;
                    TracksView.Filter += (item) => ((MediaPropertiesLibrary.Audio.Track)item).Name.ToLower().Contains(SearchText.ToLower());
                    TracksView.CustomSort = this;
                    TracksView.Refresh();
                    OnPropertyChanged(nameof(TracksView));
                }), DispatcherPriority.DataBind);
        }

        #endregion

        private UiCommand _playTrack = null;
        public UiCommand PlayTrack { get { return _playTrack; } set { _playTrack = value; OnPropertyChanged(nameof(PlayTrack)); } }

        private void Play(Track track)
        {
            switch (track.State)
            {
                case MediaState.Stopped:
                    Dispatcher.GetInstance.Dispatch("Multiple Track Selected For Play",
                        TracksView.Cast<TrackDefinition>(), TracksView.Cast<TrackDefinition>().ToList().FindIndex(o => o == track));
                    break;
                case MediaState.Playing:
                    Dispatcher.GetInstance.Dispatch("Pause");
                    break;
                default:
                    Dispatcher.GetInstance.Dispatch("Play");
                    break;
            }
        }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UiCommand ShowArtist { get; } = new UiCommand(track => Dispatcher.GetInstance.Dispatch("AudioLibrary: View Artist", ((MediaPropertiesLibrary.Audio.Track)track).Artist));
        public UiCommand ShowAlbum { get; } = new UiCommand(track => Dispatcher.GetInstance.Dispatch("AudioLibrary: View Album", ((MediaPropertiesLibrary.Audio.Track)track).Album));

        #endregion
    }
}