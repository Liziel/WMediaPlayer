using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using SharedDispatcher;
using SharedProperties.Customization;

namespace SidePlayer.MediaPlayer
{
    /// <summary>
    /// Interaction logic for MusicPlayerPluginView.xaml
    /// </summary>
    public partial class MusicPlayerPluginView : UserControl, IPlugin, IForwardDispatcher
    {
        public MusicPlayerPluginView(Uri media, TagLib.File tag)
        {
            this.DataContext = new MusicPlayerPluginViewModel(media, tag);
            this.ForwardListeners.Add(this.DataContext);
            InitializeComponent();
        }

        #region IPlugin Properties

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;
        public string PluginName { get; } = "Music Player";
        public Uri PluginIcon { get; } = null;

        #endregion

        #region IForward Dispatcher Properties

        public List<object> ForwardListeners { get; } = new List<object>();

        #endregion
    }
}
