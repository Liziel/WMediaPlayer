using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using MediaLibrary.Audio.Library;
using MediaPropertiesLibrary.Audio;
using UiLibrary;

namespace MediaLibrary.Audio.SubViews
{
    public sealed class AudioTrackViewModel : Listener, INotifyPropertyChanged
    {
        #region Track Access and Constructor

        private List<Track> _tracksAccess = null;
        public List<Track> TracksAccess => _tracksAccess;

        public AudioTrackViewModel()
        {
            Library.Library.TracksLoaded += () =>
            {
                _tracksAccess = Library.Library.Tracks;
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(delegate { OnPropertyChanged(nameof(TracksAccess)); }), DispatcherPriority.DataBind);
            };

            PlayTrack = new UiCommand(o => Play(o as Track));
        }

        #endregion

        private UiCommand _playTrack = null;
        public UiCommand PlayTrack { get { return _playTrack; } set { _playTrack = value; OnPropertyChanged(nameof(PlayTrack)); } }

        void Play(Track track)
        {
            DispatcherLibrary.Dispatcher.GetInstance.Dispatch("Audio Track Selected", track);
            DispatcherLibrary.Dispatcher.GetInstance.Dispatch("Play");
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