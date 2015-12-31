using System.Windows.Input;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class CommandItem
    {
        public string   Name { get; set; }
        public ICommand Command { get; set; }
    }
}