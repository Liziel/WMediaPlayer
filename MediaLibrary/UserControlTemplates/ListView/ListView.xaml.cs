using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaLibrary.UserControlTemplates.ListView
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : UserControl
    {
        public ListView()
        {
            InitializeComponent();
        }

        private void AudioTrackItemLoaded(object sender, RoutedEventArgs e)
        {
            var item = sender as AudioTrackListItem;
            item?.SetBinding(AudioTrackListItem.MediaPresentationColumnsProperty, new Binding("MediaPresentationColumns") {Source = this});
        }

        public List<GridLength> MediaPresentationColumns { get { return (List<GridLength>) GetValue(MediaPresentationColumnsProperty); } set { SetValue(MediaPresentationColumnsProperty, value); } }
        public static readonly DependencyProperty MediaPresentationColumnsProperty =
            DependencyProperty.Register("MediaPresentationColumns", typeof(List<GridLength>), typeof(ListView), new PropertyMetadata(new List<GridLength>()));

    }
}
