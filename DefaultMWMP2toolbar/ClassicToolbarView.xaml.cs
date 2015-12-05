using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using SharedDispatcher;
using SharedProperties.Customization;

namespace DefaultMWMP2toolbar
{
    /// <summary>
    /// Interaction logic for ClassicToolbarView.xaml
    /// </summary>
    [Export(typeof(IPlugin))]
    public partial class ClassicToolbarView : UserControl, IPlugin, IForwardDispatcher
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


        #region IPlugin Implementation

        public Position Position { get; } = Position.Bottom;
        public bool Optional { get; } = false;
        public string PluginName { get; } = nameof(ClassicToolbarView);
        public Uri PluginIcon { get; } = null;

        #endregion
    }
}
