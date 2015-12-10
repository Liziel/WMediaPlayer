using System.ComponentModel.Composition;
using System.Windows.Controls;
using LoadablePlugin;
using PluginLibrary.Customization;

namespace DefaultMWMP2MediaView
{
    /// <summary>
    /// Interaction logic for StaticView.xaml
    /// </summary>
    [Export(typeof(IViewPlugin))]
    public partial class StaticView : UserControl, ILoadableViewPlugin
    {
        public StaticView()
        {
            InitializeComponent();
        }

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
        public string PluginName { get; } = "You Fucking Tube";
        public object Logo { get; } = new YTLogo();
        public string PresentationName { get; } = "YouTube";
    }
}
