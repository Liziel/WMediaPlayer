using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MediaLibrary.Annotations;
using MediaLibrary.Audio.Pages.ArtistViewPanels;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Audio.Library;

namespace MediaLibrary.Audio.Pages
{
    public class ArtistViewModel : INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ArtistViewModel(Artist artist)
        {
            var genres =
                Library.Tracks.Where(track => track.Artists.Contains(artist)).SelectMany(track => track.Genres).Distinct().ToList();

            var t =
                Library.Tracks.Where(track => track.Genres.Intersect(genres).Any())
                    .SelectMany(track => track.Artists).Where(artist2 => artist2 != artist).Distinct()
                    .OrderBy(a => Library.Tracks.Where(track => track.Artists.Contains(a)).SelectMany(track => track.Genres).Intersect(genres).Count())
                    .ToList();

            Artist = artist;
            PopularModel = new ArtistViewPopularModel
            {
                MostListenedTracks = Library.QueryOnTrack(track => track.Artists.Contains(artist)).OrderByDescending(track => track.UserTag.TimesListened).Take(5).ToList(),
                RelatedArtists = t.Take(7).ToList()
            };
            AlbumsModel = new ArtistAlbumsViewModel(artist);
            SinglesModel = new ArtistSinglesViewModel(artist);
        }

        private Artist _artist;

        public Artist Artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public ArtistViewPopularModel   PopularModel { get; }
        public ArtistAlbumsViewModel    AlbumsModel { get; }
        public ArtistSinglesViewModel   SinglesModel { get; }
    }
}