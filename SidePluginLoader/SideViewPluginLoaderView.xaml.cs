using System.Collections.Generic;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary.Customization;
using System.ComponentModel.Composition;

namespace SidePluginLoader
{
    /// <summary>
    /// Interaction logic for SideViewPluginLoaderView.xaml
    /// </summary>
    [Export(typeof(IViewPlugin))]
    public partial class SideViewPluginLoaderView : UserControl, IViewPlugin
    {
        public SideViewPluginLoaderView()
        {
            InitializeComponent();
        }

        #region IViewPlugin

        public Position Position { get; } = Position.Left;
        public int Layer { get; } = 0;
        public bool Optional { get; } = false;

        #endregion
    }
}
