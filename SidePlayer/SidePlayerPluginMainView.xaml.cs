using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary.Customization;

namespace SidePlayer
{
    /// <summary>
    /// Interaction logic for SidePlayerPluginMainView.xaml
    /// </summary>
    public partial class SidePlayerPluginMainView : UserControl, IViewPlugin
    {
        #region Constructor

        public SidePlayerPluginMainView()
        {
            InitializeComponent();
        }

        #endregion


        #region IViewPlugin Implementation

        public Position Position { get; } = Position.Right;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;

        #endregion
    }
}
