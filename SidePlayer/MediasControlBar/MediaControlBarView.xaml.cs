using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary.Customization;

namespace SidePlayer.MediaControlBar
{
    /// <summary>
    /// Interaction logic for MediaControlBarView.xaml
    /// </summary>
    public partial class MediaControlBarView : UserControl, IViewPlugin
    {
        public MediaControlBarView()
        {
            InitializeComponent();
        }

        #region IViewPlugin Implementation

        public Position Position { get; } = Position.Invisible;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;

        #endregion

        #region On Value Changed

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var model = (MediaControlBarViewModel) DataContext;
            DispatcherLibrary.Dispatcher.GetInstance.Dispatch("Media Position Set", model.SliderCurrentValue);
        }

        #endregion
    }
}
