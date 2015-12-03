using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DefaultMWMP2toolbar
{
    /// <summary>
    /// Interaction logic for ClassicToolbarView.xaml
    /// </summary>
    [Export(typeof(SharedProperties.Interfaces.IToolBar))]
    public partial class ClassicToolbarView : UserControl, SharedProperties.Interfaces.IToolBar
    {
        public ClassicToolbarView()
        {
            InitializeComponent();
            ToolbarName = "DefaultMWMP2toolbar";
        }

        public string ToolbarName { get; }
    }
}
