using System.Windows.Controls;
using PluginLibrary.Customization;

namespace MediaLibrary.Audio
{
    /// <summary>
    /// Interaction logic for LibraryClassView.xaml
    /// </summary>
    public partial class LibraryClassView : UserControl, IViewPlugin
    {
        public LibraryClassView()
        {
            InitializeComponent();
        }

        #region ViewPlugin Properties

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;

        #endregion
    }
}
