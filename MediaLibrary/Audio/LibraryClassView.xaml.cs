using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PluginLibrary.Customization;

namespace MediaLibrary.Audio
{
    /// <summary>
    /// Interaction logic for LibraryClassView.xaml
    /// </summary>
    [Export(typeof(IViewPlugin))]
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
