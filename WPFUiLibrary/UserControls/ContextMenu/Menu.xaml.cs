using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFUiLibrary.UserControls.ContextMenu
{
    /// <summary>
    /// Interaction logic for MediaMenu.xaml
    /// </summary>
    public partial class MediaMenu : UserControl
    {
        public MediaMenu()
        {
            InitializeComponent();
            MenuContainer.SetBinding(DataContextProperty, new Binding("Menu") {Source = this});
        }

        public MenuModel Menu { get; set; }
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(MenuModel), typeof(MediaMenu));

    }
}
