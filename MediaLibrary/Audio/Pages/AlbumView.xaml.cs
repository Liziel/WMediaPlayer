using System.Windows.Controls;

namespace MediaLibrary.Audio.Pages
{
    /// <summary>
    /// Interaction logic for AlbumView.xaml
    /// </summary>
    public partial class AlbumView : UserControl
    {
        public AlbumView(AlbumViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
