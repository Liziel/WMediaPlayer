using System.ComponentModel.Composition;
using System.Windows.Controls;
using LoadablePlugin;
using PluginLibrary.Customization;

namespace LibraryView
{
    /// <summary>
    /// Interaction logic for LibraryView.xaml
    /// </summary>
    [Export(typeof(IViewPlugin))]
    public partial class LibraryView : UserControl, ILoadableViewPlugin
    {
        public LibraryView()
        {
            InitializeComponent();
        }

        #region IViewPlugin Implementation

        public Position Position { get; } = Position.Invisible;
        public int Layer { get; } = 0;
        public bool Optional { get; } = false;
        public string PluginName { get; } = "Media Library"; //probably just for audio... see later

        #endregion

        #region ILoadablePlugi Implementation

        public object Logo { get; } = new LibraryViewLogo();
        public string PresentationName { get; } = "My Musics";

        #endregion
    }
}
