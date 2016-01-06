using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MediaLibrary.Audio.Pages.ArtistViewPanels;
using MediaPropertiesLibrary.Audio;
using MediaPropertiesLibrary.Audio.Library;
using WPFUiLibrary.UserControls.StaticDisplay;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;
using MediaState = MediaPropertiesLibrary.MediaState;

namespace MediaLibrary.Audio.Pages.ArtistViewPanels
{
    /// <summary>
    /// Interaction logic for ArtistViewHeader.xaml
    /// </summary>
    public partial class ArtistViewHeader : UserControl
    {
        public ArtistViewHeader()
        {
            InitializeComponent();
            PlayButton.Command = new UiCommand(o =>
            {
                if (Artist == null) return;
                if (Artist.State == MediaState.Paused) Dispatch("Play");
                else Dispatch("Multiple Track Selected For Play", Library.Songs.Where(track => track.Artists.Contains(Artist)), 0);
            });
            PauseButton.Command = new UiCommand(o => Dispatch("Pause"));
        }

        public Artist Artist
        {
            get { return (Artist)GetValue(ArtistProperty); }
            set { SetValue(ArtistProperty, value); }
        }

        public static readonly DependencyProperty ArtistProperty
            = DependencyProperty.Register("Artist", typeof(Artist), typeof(ArtistViewHeader), new PropertyMetadata
            {
                PropertyChangedCallback = (o, args) =>
                {
                    var header = o as ArtistViewHeader;
                    if (header?.Artist != null) header.Artist.PropertyChanged -= header.OnAlbumPropertyChanged;
                    if (header == null || args.NewValue == null) return;

                    var value = (Artist)args.NewValue;
                    header.MaskedCover.Element = new Image
                    {
                        Height = 200, Width = 200,
                        Source = AccessArtistCover.AccessCover(value),
                        Stretch = Stretch.UniformToFill
                    };
                    header.MaskedCover.Visibility = AccessArtistCover.AccessCover(value)  == null
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                    header.DefaultArtist.Visibility = AccessArtistCover.AccessCover(value) == null
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                    header.ArtistName.Text = value.Name;
                    value.PropertyChanged += header.OnAlbumPropertyChanged;
                    header.RefreshDisplay(value.State == MediaPropertiesLibrary.MediaState.Playing, header.ButtonContainer.IsMouseOver);
                }
            });

        private void OnAlbumPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Artist artist = (Artist) sender;
            Dispatcher.Invoke(
                delegate { RefreshDisplay(artist.State == MediaState.Playing, ButtonContainer.IsMouseOver); });
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
            if (Artist != null)
                RefreshDisplay(Artist.State == MediaPropertiesLibrary.MediaState.Playing, true);
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Artist != null)
                RefreshDisplay(Artist.State == MediaPropertiesLibrary.MediaState.Playing, false);
        }
    }
}
