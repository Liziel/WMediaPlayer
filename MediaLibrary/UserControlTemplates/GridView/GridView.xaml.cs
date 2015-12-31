using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediaLibrary.UserControlTemplates.GridView
{
    /// <summary>
    /// Interaction logic for GridView.xaml
    /// </summary>
    public partial class GridView : UserControl
    {
        public GridView()
        {
            InitializeComponent();
            Viewer.PreviewMouseWheel += PreviewMouseWheel;
        }

        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled && Viewer.ScrollableHeight == 0)
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
