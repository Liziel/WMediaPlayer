using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using SharedDispatcher;
using SidePlayer.Annotations;

namespace SidePlayer.MediaPlayer
{
    public class MusicPlayerPluginViewModel : Listener, INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private BitmapImage _albumCover = null;
        public BitmapImage AlbumCover { get { return _albumCover; } set { _albumCover = value; OnPropertyChanged(nameof(AlbumCover)); } }

        private string _mediaName = "";
        public string MediaName { get { return _mediaName; } set { _mediaName = value; OnPropertyChanged(nameof(MediaName)); } }

        private MediaElement    _music;
        public MediaElement     Music { get { return _music; } set { _music = value; OnPropertyChanged(nameof(Music)); } }

        private TagLib.File     _tag;
    
        public MusicPlayerPluginViewModel(Uri media, TagLib.File tag)
        {
            _music = new MediaElement {Source = media, LoadedBehavior = MediaState.Manual};
            _tag = tag;

            InitializeCover();
            InializeTitle(Path.GetFileNameWithoutExtension(media.LocalPath));
        }

        private void InitializeCover()
        {
            if (_tag.Tag.IsEmpty || _tag.Tag.Pictures.Length == 0)
                return;

            var picture = _tag.Tag.Pictures[0];
            MemoryStream mstream = new MemoryStream(picture.Data.Data);
            mstream.Seek(0, SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = mstream;
            bitmap.EndInit();

            AlbumCover = bitmap;
        }

        private void InializeTitle(string filename)
        {
            if (!_tag.Tag.IsEmpty && !string.IsNullOrEmpty(_tag.Tag.Title))
                MediaName = _tag.Tag.Title;
            else
                MediaName = filename;
        }
    }
}