using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UiLibrary.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        public Page(UIElement model, ICommand sideAreaClick)
        {
            InitializeComponent();
            if (sideAreaClick != null)
                SideArea.Command = sideAreaClick;
            ContainerBorder.Child = model;
            OnHide();
        }

        public void OnHide()
        {
            SideAreaSize.Width = new GridLength(0);
        }

        public void OnShow()
        {
            SideAreaSize.Width = new GridLength(1, GridUnitType.Star);
        }
    }
}
