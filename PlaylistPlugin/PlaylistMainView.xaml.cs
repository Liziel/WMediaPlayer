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
using PluginLibrary.Customization;

namespace PlaylistPlugin
{
    /// <summary>
    /// Interaction logic for PlaylistMainView.xaml
    /// </summary>
    public partial class PlaylistMainView : UserControl, IViewPlugin
    {
        public PlaylistMainView()
        {
            InitializeComponent();
        }

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
    }
}
