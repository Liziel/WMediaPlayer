using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFUiLibrary.Utils;

namespace WPFUiLibrary.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for PagesAnchorer.xaml
    /// </summary>
    public partial class PagesAnchorer : UserControl
    {
        public ObservableCollection<UIElement> Pages
        {
            get { return GetValue(PagesProperty) as ObservableCollection<UIElement>; }
            set
            {
                SetValue(PagesProperty, value);
            }
        }

        public static readonly DependencyProperty PagesProperty = 
            DependencyProperty.Register("Pages", typeof(ObservableCollection<UIElement>), typeof(PagesAnchorer), new PropertyMetadata(OnPagesChanged));

        private static void OnPagesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _this = d as PagesAnchorer;
            var value = e.NewValue as ObservableCollection<UIElement>;
            var old = e.OldValue as ObservableCollection<UIElement>;

            if (old != null)
                old.CollectionChanged -= _this.PagesChangeHandler;
            if (value != null)
                value.CollectionChanged += _this.PagesChangeHandler;
        }

        private void PagesChangeHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    WPFUiLibrary.UserControls.Pages.Page page = null;
                    if (ContainerGrid.Children.Count > 1)
                        page = ContainerGrid.Children.Cast<WPFUiLibrary.UserControls.Pages.Page>().Reverse().Skip(1).First();
                    if (ContainerGrid.Children.Count > 0)
                        ContainerGrid.Children.Cast<WPFUiLibrary.UserControls.Pages.Page>().Last().OnHide(page);
                    foreach (var newItem in e.NewItems)
                    {
                        ContainerGrid.Children.Add(new WPFUiLibrary.UserControls.Pages.Page(newItem as UIElement,
                            new UiCommand(o => Pages.RemoveAt(Pages.Count - 1))));
                    }
                    ContainerGrid.Children.Cast<WPFUiLibrary.UserControls.Pages.Page>().Last()?.OnShow();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ContainerGrid.Children.RemoveAt(e.OldStartingIndex);
                    if (ContainerGrid.Children.Count > 0)
                        ContainerGrid.Children.Cast<WPFUiLibrary.UserControls.Pages.Page>().Last().OnShow();
                    if (ContainerGrid.Children.Count > 1)
                        ContainerGrid.Children.Cast<WPFUiLibrary.UserControls.Pages.Page>().Reverse().Skip(1).First().Visibility = Visibility.Visible;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ContainerGrid.Children.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public PagesAnchorer()
        {
            InitializeComponent();
        }
    }
}
