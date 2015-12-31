using System.Windows.Controls;

namespace MediaLibrary.Audio.SubViews
{
    /// <summary>
    /// Interaction logic for TrackView.xaml
    /// </summary>
    public partial class TrackView : UserControl
    {
        public TrackView(TrackViewModel model)
        {
            this.DataContext = model;
            InitializeComponent();
        }
    }
}
