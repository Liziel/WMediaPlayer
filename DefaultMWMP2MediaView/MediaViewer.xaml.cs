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
using DispatcherLibrary;
using PluginLibrary.Customization;

namespace DefaultMWMP2MediaView
{
    /// <summary>
    /// Interaction logic for MediaViewer.xaml
    /// </summary>
    [Export(typeof(IPlugin))]
    public partial class MediaViewer : UserControl, IPlugin, IForwardDispatcher
    {
        #region IForwardDispatcher Implementation

        public List<object> ForwardListeners { get; }

        #endregion

        #region Constructor

        public MediaViewer()
        {
            InitializeComponent();
            ForwardListeners = new List<object>() {this.DataContext};
        }

        #endregion

        #region IPlugin Implementation

        public Position Position { get; } = Position.All;
        public int Layer { get; } = 1;
        public bool Optional { get; } = true;
        public string PluginName { get; } = nameof(MediaViewer);
        public Uri PluginIcon { get; } = null;

        #endregion
    }
}
