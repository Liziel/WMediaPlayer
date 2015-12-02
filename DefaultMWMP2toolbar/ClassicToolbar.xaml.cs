using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace DefaultMWMP2toolbar
{
    /// <summary>
    /// Interaction logic for ClassicToolbar.xaml
    /// </summary>
    [Export(typeof(SharedProperties.Interfaces.IToolBar))]
    public partial class ClassicToolbar : UserControl, SharedProperties.Interfaces.IToolBar
    {
        public ClassicToolbar()
        {
            InitializeComponent();
            Name = typeof (ClassicToolbar).Name;
        }
    }
}
