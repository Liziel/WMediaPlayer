using System.Collections.Generic;
using System.Collections.ObjectModel;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.Audio.Pages.ArtistViewPanels
{
    public class ArtistViewPopularModel
    {
        public List<Track> MostListenedTracks { get; set; }
        public List<Artist> RelatedArtists { get; set; }   
    }
}