using System.Windows.Controls;

namespace SidePlayer.MediasPlayer.Audio
{
    /// <summary>
    /// Interaction logic for MusicView.xaml
    /// </summary>
    public partial class MusicView : UserControl
    {
        public MusicView(MusicPlayerPluginViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
