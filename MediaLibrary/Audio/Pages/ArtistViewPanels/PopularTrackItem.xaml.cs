using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaLibrary.Audio.Pages.ArtistViewPanels
{
    /// <summary>
    /// Interaction logic for PopularTrackItem.xaml
    /// </summary>
    public partial class PopularTrackItem : UserControl
    {
        public PopularTrackItem()
        {
            InitializeComponent();
            PlayMusicButton.LaunchCommand = PlayArtist;
            PlayMusicButton.LaunchCommandParameter = DataContext;
        }

        public ICommand PlayArtist { get; set; }
        static public readonly DependencyProperty PlayArtistProperty = DependencyProperty.Register("PlayArtist", typeof(ICommand), typeof(PopularTrackItem), new PropertyMetadata
        {
            PropertyChangedCallback  = (o, args) =>
            {
                var item = o as PopularTrackItem;
                if (item == null) return;

                item.PlayMusicButton.LaunchCommand = item.PlayArtist;
            }
        });

    }
}
