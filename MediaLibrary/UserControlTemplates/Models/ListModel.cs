using System.Collections.Generic;
using System.Windows.Data;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.UserControlTemplates.Models
{
    public delegate void PlayAudioTrack(MediaPropertiesLibrary.Audio.Track track);
    public delegate void PlayVideoTrack(MediaPropertiesLibrary.Video.Track track);
    public delegate void PlayArtist(MediaPropertiesLibrary.Audio.Artist track);
    public delegate void PlayAlbum(MediaPropertiesLibrary.Audio.Album track);
    public class ListModel
    {
        public PlayAudioTrack PlayAudioTrack { get; set; } = null;
        public PlayAlbum PlayAlbum { get; set; } = null;
        public PlayArtist PlayArtist { get; set; } = null;

        public PlayVideoTrack PlayVideoTrack { get; set; } = null;

        public ListCollectionView   List { get; set; } = null;
    }
}