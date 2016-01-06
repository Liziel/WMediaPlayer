using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFUiLibrary.UserControls.ContextMenu;
using WPFUiLibrary.UserControls.MediaTemplates.ListView;
using Button = WPFUiLibrary.UserControls.ContextMenu.Button;

namespace WPFUiLibrary.UserControls.MediaTemplates.GridView
{
    /// <summary>
    /// Interaction logic for PicturesFolderGridItem.xaml
    /// </summary>
    public partial class PicturesFolderGridItem : UserControl
    {
        public PicturesFolderGridItem()
        {
            InitializeComponent();
        }

        public MenuModel Menu { get { return (MenuModel)GetValue(MenuProperty); } set { SetValue(MenuProperty, value); } }
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(MenuModel), typeof(PicturesFolderGridItem));

        private void ContextMenu(object sender, RoutedEventArgs e)
        {
            var contextMenuButton = sender as Button;
            contextMenuButton?.SetBinding(Button.MenuProperty, new Binding("Menu") {Source = this});
        }
    }
}
