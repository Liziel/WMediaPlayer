using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPropertiesLibrary;
using WPFUiLibrary.UserControls.ContextMenu.MenuItems;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.ContextMenu.BaseItems
{
    /// <summary>
    /// Interaction logic for CreatePlaylist.xaml
    /// </summary>
    public partial class CreatePlaylist : UserControl, IMenuClosable
    {
        public CreatePlaylist(Action<string> create)
        {
            InitializeComponent();
            Create = delegate
            {
                if (string.IsNullOrWhiteSpace(TextBox.Text)) return;
                create(TextBox.Text);
                Close();
            };

            Button.Command = new UiCommand( o => Create() );
        }

        public Action Close { private get; set; }
        private Action Create { get; }

        private void EscapeCatch(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
            if (e.Key == Key.Enter) Create();
        }


    }
}
