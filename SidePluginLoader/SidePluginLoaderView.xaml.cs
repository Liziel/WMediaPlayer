using System.Collections.Generic;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary.Customization;
using System.ComponentModel.Composition;

namespace SidePluginLoader
{
    /// <summary>
    /// Interaction logic for SidePluginLoaderView.xaml
    /// </summary>
    [Export(typeof(IPlugin))]
    public partial class SidePluginLoaderView : UserControl, IForwardDispatcher, IPlugin
    {
        public SidePluginLoaderView()
        {
            InitializeComponent();
            ForwardListeners = new List<object> {DataContext};
        }

        #region Forward Dispatcher

        public List<object> ForwardListeners { get; }

        #endregion

        #region IPlugin

        public Position Position { get; } = Position.Left;
        public int Layer { get; } = 0;
        public bool Optional { get; } = false;
        public string PluginName { get; } = "Plugin Loader";

        #endregion
    }
}
