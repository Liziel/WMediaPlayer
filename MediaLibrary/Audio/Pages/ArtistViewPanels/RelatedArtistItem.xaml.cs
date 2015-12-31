using System.Windows.Controls;
using System.Windows.Input;
using static DispatcherLibrary.Dispatcher;

namespace MediaLibrary.Audio.Pages.ArtistViewPanels
{
    /// <summary>
    /// Interaction logic for RelatedArtistItem.xaml
    /// </summary>
    public partial class RelatedArtistItem : UserControl
    {
        public RelatedArtistItem()
        {
            InitializeComponent();
        }

        private void ViewArtist(object sender, MouseButtonEventArgs e)
        {
            Dispatch("AudioLibrary: View Artist", DataContext);
        }
    }
}
