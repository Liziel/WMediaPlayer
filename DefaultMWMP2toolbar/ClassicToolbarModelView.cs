using System.ComponentModel;
using System.Runtime.CompilerServices;
using DefaultMWMP2toolbar.Annotations;
using SharedDispatcher;

namespace DefaultMWMP2toolbar
{
    public class ClassicToolbarModelView : Listener, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}