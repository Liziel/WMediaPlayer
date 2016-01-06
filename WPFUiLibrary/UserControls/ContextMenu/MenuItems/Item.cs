using System.Windows.Input;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class Item
    {
        public ICommand     Command { get; set; }
        public MenuModel    Menu { get; set; }
    }
}