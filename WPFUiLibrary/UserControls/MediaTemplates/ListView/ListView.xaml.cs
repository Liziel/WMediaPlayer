using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace WPFUiLibrary.UserControls.MediaTemplates.ListView
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            MediaPresentationColumns = new List<GridLength>();
            InitializeComponent();
        }

        private void VideoTrackItemLoaded(object sender, RoutedEventArgs e)
        {
            var item = sender as VideoTrackListItem;
            item?.SetBinding(VideoTrackListItem.MediaPresentationColumnsProperty, new Binding("MediaPresentationColumns") { Source = this });
        }

        private void AudioTrackItemLoaded(object sender, RoutedEventArgs e)
        {
            var item = sender as AudioTrackListItem;
            item?.SetBinding(AudioTrackListItem.MediaPresentationColumnsProperty, new Binding("MediaPresentationColumns") { Source = this });
        }

        public List<GridLength> MediaPresentationColumns { get { return (List<GridLength>)this.GetValue(MediaPresentationColumnsProperty); } set { SetValue(MediaPresentationColumnsProperty, value); } }
        public static readonly DependencyProperty MediaPresentationColumnsProperty =
            DependencyProperty.Register("MediaPresentationColumns", typeof(List<GridLength>), typeof(ListView), new PropertyMetadata());

        private void BubleScroll(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled && ( (ScrollViewer)sender ).ScrollableHeight == 0.0)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
