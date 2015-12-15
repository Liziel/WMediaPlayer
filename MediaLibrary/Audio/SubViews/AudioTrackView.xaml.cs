using System.Windows.Controls;

namespace MediaLibrary.Audio.SubViews
{
    /// <summary>
    /// Interaction logic for AudioTrackView.xaml
    /// </summary>
    public partial class AudioTrackView : UserControl
    {
        public AudioTrackView(AudioTrackViewModel model)
        {
            this.DataContext = model;
            InitializeComponent();
        }
    }
}
