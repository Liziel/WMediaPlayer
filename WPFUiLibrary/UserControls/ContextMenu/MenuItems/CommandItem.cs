using System;
using System.Windows.Input;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class CommandItem : Item
    {
        public string   Name { get; set; }
        public Action   Action { get; set; }
    }
}