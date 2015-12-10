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

namespace SidePlayer.MediasPlayer.Video
{
    /// <summary>
    /// Interaction logic for VideoPlayerViewPluginView.xaml
    /// </summary>
    public partial class VideoPlayerViewPluginView : UserControl, IViewPlugin, INotifyPropertyChanged
    {
        public VideoPlayerViewPluginView()
        {
            InitializeComponent();
        }

        #region IViewPlugin Properties

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;

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

        private void OnArtistsTextUpdated(object sender, EventArgs e)
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

        private void ResearchAlbum(object sender, MouseButtonEventArgs e)
        {
        }

        private void ResearchArtists(object sender, MouseButtonEventArgs e)
        {
        }

        private void MaximizeVideoPlayer(object sender, MouseButtonEventArgs e)
        {
            DispatcherLibrary.Dispatcher.GetInstance.Dispatch("Maximize Media View");
        }
    }
}
