using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DispatcherLibrary;
using PluginLibrary.Customization;
using SidePlayer.Annotations;

namespace SidePlayer.MediasPlayer.Audio
{
    /// <summary>
    /// Interaction logic for MusicPlayerPluginView.xaml
    /// </summary>
    public partial class MusicPlayerPluginView : UserControl, IPlugin, IForwardDispatcher, INotifyPropertyChanged
    {
        public MusicPlayerPluginView(Uri media, TagLib.File tag)
        {
            this.DataContext = new MusicPlayerPluginViewModel(media, tag);
            this.ForwardListeners.Add(this.DataContext);
            InitializeComponent();
        }

        #region IPlugin Properties

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
        public string PluginName { get; } = "Video Player";
        public Uri PluginIcon { get; } = null;

        #endregion

        #region IForward Dispatcher Properties

        public List<object> ForwardListeners { get; } = new List<object>();

        #endregion

        #region Layout Resizing and Animation

        private void OnTitleAnimationCompleted(object sender, EventArgs e)
        {
            if (TitleBlock.IsMouseOver)
                (TitleBlock.FindResource("TitleSlide") as Storyboard)?.Begin();
        }

        private void OnTitleTextUpdated(object sender, EventArgs e)
        {
            if (TitleTestSize.ActualWidth > 200)
            {
                TitleTestSize.Visibility = Visibility.Hidden;
                TitleBlock.Visibility = Visibility.Visible;
            }
            else
            {
                TitleBlock.Visibility = Visibility.Hidden;
                TitleTestSize.Visibility = Visibility.Visible;
            }
        }

        private void OnArtistAnimationCompleted(object sender, EventArgs e)
        {
            if (ArtistBlock.IsMouseOver)
                (ArtistBlock.FindResource("ArtistSlide") as Storyboard)?.Begin();
        }

        private void OnArtistTextUpdated(object sender, EventArgs e)
        {
            if (ArtistTestSize.ActualWidth > 200)
            {
                ArtistTestSize.Visibility = Visibility.Hidden;
                ArtistBlock.Visibility = Visibility.Visible;
            }
            else
            {
                ArtistBlock.Visibility = Visibility.Hidden;
                ArtistTestSize.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void AlbumResearch(object sender, MouseButtonEventArgs e)
        {

        }

        private void ArtistResearch(object sender, MouseButtonEventArgs e)
        {

        }

        private void MaximizeVideoPlayer(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
