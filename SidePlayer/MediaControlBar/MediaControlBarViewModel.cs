using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharedDispatcher;
using SidePlayer.Annotations;

namespace SidePlayer.MediaControlBar
{
    public class MediaControlBarViewModel : Listener , INotifyPropertyChanged
    {
        #region Notifier Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}