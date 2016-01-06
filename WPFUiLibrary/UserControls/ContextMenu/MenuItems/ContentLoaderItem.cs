using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WPFUiLibrary.UserControls.ContextMenu.MenuItems
{
    public class ContentLoaderItem : Item
    {
        public string               Name    { get; set; }
        public Func<IEnumerable<Item>>    Items { get; set; }
    }
}