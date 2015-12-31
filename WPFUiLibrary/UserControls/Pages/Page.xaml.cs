using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace WPFUiLibrary.UserControls.Pages
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
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var sb = FindResource("OnLoadAnimation") as Storyboard;
            Loaded -= OnLoaded;
            sb.Begin();
        }

        public void OnHide(Page page)
        {
            var sb = FindResource("OnHide") as Storyboard;
            if (page != null)
                sb.Completed += (sender, args) => page.Visibility = Visibility.Collapsed;
            sb.Begin();
        }

        public void OnShow()
        {
            var sb = FindResource("OnShow") as Storyboard;
            sb.Begin();
        }

        public void OnExit(Action action)
        {
            var sb = FindResource("OnExit") as Storyboard;
            sb.Completed += (sender, args) => action();
        }
    }
}
