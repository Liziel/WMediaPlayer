using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary.Customization;

namespace DefaultMWMP2toolbar
{
    /// <summary>
    /// Interaction logic for ClassicToolbarView.xaml
    /// </summary>
    [Export(typeof(IViewPlugin))]
    public partial class ClassicToolbarView : UserControl, IViewPlugin
    {
        #region IForwardDispatcher Implementation

        public List<object> ForwardListeners { get; }

        #endregion

        #region Constructor

        public ClassicToolbarView()
        {
            InitializeComponent();
            ForwardListeners = new List<object>() {this.DataContext};
        }

        #endregion


        #region IViewPlugin Implementation

        public Position Position { get; } = Position.Invisible;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
        public string PluginName { get; } = nameof(ClassicToolbarView);
        public Uri PluginIcon { get; } = null;

        #endregion
    }
}
