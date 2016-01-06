using System.Windows.Controls;
using PluginLibrary.Customization;

namespace MyPicturesPlugin.Views
{
    /// <summary>
    /// Interaction logic for PicturesView.xaml
    /// </summary>
    public partial class PicturesView : UserControl, IViewPlugin
    {
        public PicturesView()
        {
            InitializeComponent();
        }

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
    }
}
