using System.Windows.Controls;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class UserControlItem : Item
    {
        public IMenuClosable UserControl { get; set; }
    }
}