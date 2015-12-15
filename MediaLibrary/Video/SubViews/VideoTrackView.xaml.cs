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

namespace MediaLibrary.Video.SubViews
{
    /// <summary>
    /// Interaction logic for VideoTrackView.xaml
    /// </summary>
    public partial class VideoTrackView : UserControl
    {
        public VideoTrackView(object model)
        {
            DataContext = model;
            InitializeComponent();
        }
    }
}
