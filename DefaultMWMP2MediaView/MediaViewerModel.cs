using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;
using DefaultMWMP2MediaView.Annotations;
using DispatcherLibrary;

namespace DefaultMWMP2MediaView
{
    public class MediaViewerModel : Listener, INotifyPropertyChanged
    {
        #region ViewModel Properties

        private object _currentViewModel;

        [ForwardDispatch]
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

        public MediaViewerModel() : base()
        {
            this.CurrentViewModel = new StaticViewModel();
        }

        [EventHook("Media Opening")]
        public void OnMediaLoad(Uri media)
        {
            this.CurrentViewModel = new MediaDisplayViewModel(media);
        }

        [EventHook("Media Closing")]
        public void OnMediaClose()
        {
            this.CurrentViewModel = new StaticViewModel();
        }
    }
}