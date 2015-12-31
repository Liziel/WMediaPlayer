using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
