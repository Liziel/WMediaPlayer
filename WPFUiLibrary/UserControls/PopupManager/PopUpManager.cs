using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using DispatcherLibrary;

namespace WPFUiLibrary.UserControls.PopupManager
{
    public class PopUpManager : Listener
    {
        private readonly ObservableCollection<PopUp> _popUps = new ObservableCollection<PopUp>(); 

        private readonly CollectionViewSource _popUpCollectionView;
        public ListCollectionView PopUps => _popUpCollectionView?.View as ListCollectionView;

        public PopUpManager()
        {
            _popUpCollectionView = new CollectionViewSource {Source = _popUps};
            Dispatcher.AddEventListener(this);
        }

        [EventHook("Add PopUp")]
        public void AddPopUp(PopUp popUp)
        {
            _popUps.Add(popUp);
        }

        [EventHook("Remove PopUp")]
        public void RemovePopUp(PopUp popUp)
        {
            _popUps.Remove(popUp);
        }

        [EventHook("Remove PopUps")]
        public void RemovePopUps(Func<PopUp, bool> remover)
        {
            foreach (var item in _popUps.Where(remover).ToList())
                _popUps.Remove(item);
        }
    }
}