using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPropertiesLibrary.Audio;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;
using MediaState = MediaPropertiesLibrary.MediaState;

namespace MediaLibrary.Audio.Pages.AlbumViewPanels
{
    /// <summary>
    /// Interaction logic for AlbumViewHeader.xaml
    /// </summary>
    public partial class AlbumViewHeader : UserControl
    {
        public AlbumViewHeader()
        {
            InitializeComponent();
            PlayButton.Command = new UiCommand(o =>
            {
                if (Album == null) return;
                if (Album.State == MediaState.Paused) Dispatch("Play");
                else Dispatch("Multiple Track Selected For Play", Album.Tracks, 0);
            });
            PauseButton.Command = new UiCommand(o => Dispatch("Pause"));
        }

        public Album Album
        {
            get { return (Album) GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }

        public static readonly DependencyProperty AlbumProperty
            = DependencyProperty.Register("Album", typeof(Album), typeof(AlbumViewHeader), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var header = o as AlbumViewHeader;
                    if (header == null) return;
                    if (header.Album != null) header.Album.PropertyChanged -= header.OnAlbumPropertyChanged;
                    if (args.NewValue == null) return;

                    var value = (Album)args.NewValue;
                    header.Cover.Source = value.Cover;
                    header.AlbumTitle.Text = value.Name;
                    value.PropertyChanged += header.OnAlbumPropertyChanged;
                    header.RefreshDisplay(value.State == MediaState.Playing, header.ButtonContainer.IsMouseOver);
                }
            });

        private void OnAlbumPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            try
            {
                if (Album != null)
                    RefreshDisplay(Album.State == MediaState.Playing,
                        ButtonContainer.IsMouseOver);
            }
            catch (InvalidOperationException)
            {
                return;
            }
        }

        private void RefreshDisplay(bool inPlay, bool mouseOver)
        {
            InPlayButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Collapsed;

            if (inPlay && mouseOver) PauseButton.Visibility = Visibility.Visible;
            else if (inPlay) InPlayButton.Visibility = Visibility.Visible;
            else PlayButton.Visibility = Visibility.Visible;
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Album != null)
                RefreshDisplay(Album.State == MediaState.Playing, true);
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Album != null)
                RefreshDisplay(Album.State == MediaState.Playing, false);
        }
    }
}
