using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DefaultMWMP2MediaView.Annotations;
using SharedDispatcher;

namespace DefaultMWMP2MediaView
{
    public class MediaViewerModel : Listener, INotifyPropertyChanged
    {
        #region ViewModel Properties

        private object _currentViewModel;

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        #endregion

        #region Notifier Content

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public MediaViewerModel()
        {
            this.CurrentViewModel = new StaticViewModel();
        }

        [EventHook("Media Opening")]
        void OnMediaLoad(Uri media)
        {
        }
    }
}