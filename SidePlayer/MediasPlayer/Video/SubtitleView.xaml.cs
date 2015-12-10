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

namespace SidePlayer.MediasPlayer.Video
{
    /// <summary>
    /// Interaction logic for SubtitleView.xaml
    /// </summary>
    public partial class SubtitleView : UserControl
    {
        public SubtitleView(Subtitles context)
        {
            DataContext = context;
            InitializeComponent();
        }
    }
}
