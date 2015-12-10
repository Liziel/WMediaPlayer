using System.Windows.Controls;
using PluginLibrary.Customization;

namespace SidePlayer.MaximizedMediaPlayer
{
    /// <summary>
    /// Interaction logic for MaximizedMediaPlayerView.xaml
    /// </summary> 
    public partial class MaximizedMediaPlayerView : UserControl, IViewPlugin
    {
        public MaximizedMediaPlayerView(MaximizedMediaPlayerViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }

        public Position Position { get; } = Position.All;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
    }
}
