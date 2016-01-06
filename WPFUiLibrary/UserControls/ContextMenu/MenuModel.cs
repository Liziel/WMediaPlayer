using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using MediaPropertiesLibrary;
using MediaPropertiesLibrary.Annotations;
using WPFUiLibrary.UserControls.ContextMenu.MenuItems;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.ContextMenu
{
    public class MenuModel : INotifyPropertyChanged
    {
        public MenuModel(bool generic = true)
        {
            Precedent = new UiCommand(o => PopItems());
            _menuItems = ItemConfigurator.Generate(new List<Item>(), this, generic);
        }

        public MenuModel(IEnumerable<Item> items, bool generic = true)
        {
            Precedent = new UiCommand(o => PopItems());
            _menuItems = ItemConfigurator.Generate(items, this, generic);
        }

        #region Notifier Fields

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public IEnumerable<TrackDefinition>     Tracks { get; set; }


        private readonly IEnumerable<Item>           _menuItems;
        private readonly List<IEnumerable<Item>>     _menuStack = new List<IEnumerable<Item>>();
        private readonly List<string>                _menuTitles = new List<string>();
         
        public  IEnumerable<Item>   MenuItems => _menuStack.Count == 0 ? _menuItems : _menuStack[_menuStack.Count - 1];
        public string               MenuTitle => _menuTitles.Count > 0 ? _menuTitles[_menuTitles.Count - 1] : null;

        public void PushItems(IEnumerable<Item> items, string name)
        {
            _menuStack.Add(items);
            _menuTitles.Add(name);
            OnPropertyChanged(nameof(MenuItems));
            OnPropertyChanged(nameof(MenuTitle));
        }

        private void PopItems()
        {
            _menuStack.RemoveAt(_menuStack.Count - 1);
            _menuTitles.RemoveAt(_menuTitles.Count - 1);
            OnPropertyChanged(nameof(MenuItems));
            OnPropertyChanged(nameof(MenuTitle));
        }
        public ICommand Precedent { get; }

        public void Reset()
        {
            _menuStack.Clear();
            _menuTitles.Clear();
            OnPropertyChanged(nameof(MenuItems));
            OnPropertyChanged(nameof(MenuTitle));
        }
    }
}