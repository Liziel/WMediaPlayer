using System.Windows.Controls;
using PluginLibrary.Customization;

namespace SidePluginLoader
{
    /// <summary>
    /// Interaction logic for CenterLoadableView.xaml
    /// </summary>
    public partial class CenterLoadableView : UserControl, IViewPlugin
    {
        public CenterLoadableView()
        {
            InitializeComponent();
        }

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = false;
    }
}
