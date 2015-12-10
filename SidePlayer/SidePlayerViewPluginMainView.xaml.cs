using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary.Customization;

namespace SidePlayer
{
    /// <summary>
    /// Interaction logic for SidePlayerViewPluginMainView.xaml
    /// </summary>
    public partial class SidePlayerViewPluginMainView : UserControl, IViewPlugin
    {
        #region Constructor

        public SidePlayerViewPluginMainView(object viewModel)
        {
            DataContext = viewModel;
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
