using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using PluginLibrary.Customization;

namespace SidePlayer.MaximizedMediaPlayer
{
    /// <summary>
    /// Interaction logic for MaximizedMediaPlayerView.xaml
    /// </summary> 
    public partial class MaximizedMediaPlayerView : UserControl, IViewPlugin
    {
        public MaximizedMediaPlayerView(MaximizedMediaPlayerViewModel model)
        {
            DataContext = model;
            InitializeComponent();
        }

        public Position Position { get; } = Position.All;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            var storyboard = (Storyboard) FindResource("HidingStoryBoard");

            if (!ControlBar.IsMouseOver)
                storyboard.Seek(TimeSpan.Zero);
        }

        private void ControlBar_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("HidingStoryBoard");

            storyboard.Seek(TimeSpan.Zero);
            storyboard.Pause();
        }

        private void ControlBar_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var storyboard = (Storyboard)FindResource("HidingStoryBoard");

            storyboard.Begin();
        }
    }
}
