using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Audio.Library;
using WPFUiLibrary.Utils;
using Dispatcher = DispatcherLibrary.Dispatcher;

namespace MediaLibrary.Audio.SubViews
{
    public class ArtistViewModel : Listener, INotifyPropertyChanged
    {
        private readonly CollectionViewSource _artistCollectionViewSource = new CollectionViewSource();
        public ListCollectionView ArtistsView => _artistCollectionViewSource.View as ListCollectionView;

        private string _searchText = "";

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ArtistsView?.Refresh();
            }
        }

        public ArtistViewModel()
        {
            _artistCollectionViewSource.Source = MediaPropertiesLibrary.Audio.Library.Library.Artists;
            ArtistsView.Filter += item => ((Artist)item).Name.ToLower().Contains(SearchText.ToLower());
            ArtistsView.CustomSort =
                Comparer<Artist>.Create(
                    (artist1, artist2) =>
                        string.Compare(artist1.Name, artist2.Name, StringComparison.CurrentCultureIgnoreCase));
            ArtistsView.Refresh();
            OnPropertyChanged(nameof(ArtistsView));
        }

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public UiCommand PlayArtist { get; } = new UiCommand(o => Dispatcher.Dispatch("Multiple Track Selected For Play", Library.QueryOnTrack(track => track.Artists.Contains(o)), 0));
        public UiCommand SelectArtist { get; } = new UiCommand(o => Dispatcher.Dispatch("AudioLibrary: View Artist", o));

    }
}