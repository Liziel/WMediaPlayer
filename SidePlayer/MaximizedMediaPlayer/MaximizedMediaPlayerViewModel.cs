using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using SidePlayer.Annotations;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace SidePlayer.MaximizedMediaPlayer
{
    public class MaximizedMediaPlayerViewModel : INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region User Controls

        private UserControl _mediaDisplay;

        public UserControl MediaDisplay
        {
            get { return _mediaDisplay; }
            set
            {
                _mediaDisplay = value;
                OnPropertyChanged(nameof(MediaDisplay));
            }
        }

        private UserControl _mediaControl;

        public UserControl MediaControl
        {
            get { return _mediaControl; }
            set
            {
                _mediaControl = value;
                OnPropertyChanged(nameof(MediaControl));
            }
        }

        #endregion

        public UiCommand Minimize { get; } = new UiCommand(delegate {Dispatch("Minimize Media View");});

        public MaximizedMediaPlayerViewModel(UserControl mediaDisplay, UserControl mediaControl)
        {
            MediaControl = mediaControl;
            MediaDisplay = mediaDisplay;
        }
    }
}