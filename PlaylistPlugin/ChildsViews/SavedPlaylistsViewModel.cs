﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DispatcherLibrary;
using MediaLibrary.Annotations;
using PlaylistPlugin.Models;

namespace PlaylistPlugin.ChildsViews
{
    public class SavedPlaylistsViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private List<Playlist>  _savedPlaylists;
        public List<Playlist>   SavedPlaylists
        {
            get { return _savedPlaylists; }
            set
            {
                _savedPlaylists = value;
                OnPropertyChanged(nameof(SavedPlaylists));
            }
        }

        [EventHook("Save Playlist")]
        public void OnSavedPlaylist(Playlist playlist)
        {
            SavedPlaylists.Add(playlist);
            OnPropertyChanged(nameof(SavedPlaylists));
        }

    }
}