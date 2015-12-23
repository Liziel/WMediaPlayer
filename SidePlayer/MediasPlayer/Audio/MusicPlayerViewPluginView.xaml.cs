using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DispatcherLibrary;
using PluginLibrary.Customization;
using SidePlayer.Annotations;

namespace SidePlayer.MediasPlayer.Audio
{
    /// <summary>
    /// Interaction logic for MusicPlayerViewPluginView.xaml
    /// </summary>
    public partial class MusicPlayerViewPluginView : UserControl, INotifyPropertyChanged
    {

        public MusicPlayerViewPluginView()
        {
            InitializeComponent();
        }

        #region IViewPlugin Properties

        public Position Position { get; } = Position.Center;
        public int Layer { get; } = 0;
        public bool Optional { get; } = true;

        #endregion

        #region IForward Dispatcher Properties

        public List<object> ForwardListeners { get; } = new List<object>();

        #endregion

        #region Layout Resizing and Animation

        private void OnArtistAnimationCompleted(object sender, EventArgs e)
        {
            if (ArtistBlock.IsMouseOver)
                (ArtistBlock.FindResource("ArtistSlide") as Storyboard)?.Begin();
        }

        #endregion

        private void MaximizeAudioPlayer(object sender, MouseButtonEventArgs e)
        {
            DispatcherLibrary.Dispatcher.GetInstance.Dispatch("Maximize Media View");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
