using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaPropertiesLibrary.Audio;

namespace MediaLibrary.Audio.SubViews
{
    public class AudioAlbumViewModel : Listener, INotifyPropertyChanged
    {
        private List<Album> _albumsAccess = null;
        public List<Album> AlbumsAccess => _albumsAccess;

        public AudioAlbumViewModel()
        {
            Library.Library.TracksLoaded += () =>
            {
               _albumsAccess = Library.Library.Albums;
               Application.Current.Dispatcher.BeginInvoke(new Action(delegate { OnPropertyChanged(nameof(AlbumsAccess)); }), DispatcherPriority.DataBind);
            };
        }

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