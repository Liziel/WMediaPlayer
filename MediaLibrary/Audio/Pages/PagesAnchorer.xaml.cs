using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UiLibrary;

namespace MediaLibrary.Audio.Pages
{
    /// <summary>
    /// Interaction logic for PagesAnchorer.xaml
    /// </summary>
    public partial class PagesAnchorer : UserControl
    {
        public ObservableCollection<UIElement> Pages
        {
            get { return base.GetValue(PagesProperty) as ObservableCollection<UIElement>; }
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
            {
                old.CollectionChanged -= PagesChangeHandler;
            }
            if (value != null)
            {
                value.CollectionChanged += PagesChangeHandler;
                _this?.RefreshPages(value);
            }
        }

        private static void PagesChangeHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            return;
        }

        private void RefreshPages(IReadOnlyList<UIElement> value)
        {
            ContainerGrid.Children.Clear();
            for (var index = 0; index < value.Count; index++)
            {
                var uiElement = value[index];
                var page = index == value.Count - 1 ? new Page(uiElement, new UiCommand(delegate
                {
                    var parameter = Pages.Last();
                    if (PageRemoval != null && !PageRemoval.CanExecute(parameter)) return;
                    PageRemoval?.Execute(parameter);
                    RefreshPages(Pages.Take(Pages.Count - 1).ToList());
                })) : uiElement;
                Panel.SetZIndex(page, index);
                ContainerGrid.Children.Add(page);
            }
        }

        public ICommand PageRemoval
        {
            get { return base.GetValue(PageRemovalProperty) as ICommand; }
            set { SetValue(PageRemovalProperty, value); }
        }

        public static readonly DependencyProperty PageRemovalProperty =
            DependencyProperty.RegisterAttached("PageRemoval", typeof(ICommand), typeof(PagesAnchorer));

        public PagesAnchorer()
        {
            InitializeComponent();
        }
    }
}
