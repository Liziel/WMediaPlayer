using System.Windows.Controls;
using PluginLibrary;
using PluginLibrary.Customization;

namespace MediaLibrary.Video
{
    /// <summary>
    /// Interaction logic for LibraryClassView.xaml
    /// </summary>
    public partial class LibraryClassView : UserControl, IViewPlugin
    {
        public LibraryClassView(LibraryClassViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
    }
}
