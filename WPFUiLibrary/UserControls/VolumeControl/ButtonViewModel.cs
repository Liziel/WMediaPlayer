using System.ComponentModel;
using System.Runtime.CompilerServices;
using MediaPropertiesLibrary.Annotations;

namespace WPFUiLibrary.UserControls.VolumeControl
{
    public sealed class ButtonViewModel : INotifyPropertyChanged
    {
        #region Notify Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private double _volume;

        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
    }
}