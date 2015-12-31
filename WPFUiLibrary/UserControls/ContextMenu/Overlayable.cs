using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using WPFUiLibrary.UserControls.PopupManager;
using static DispatcherLibrary.Dispatcher;

namespace WPFUiLibrary.UserControls.ContextMenu
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFUiLibrary.UserControls.ContextMenu"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFUiLibrary.UserControls.ContextMenu;assembly=WPFUiLibrary.UserControls.MediaMenu"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Overlayable/>
    ///
    /// </summary>
    [ContentProperty("Child")]
    public class Overlayable : Control
    {
        static Overlayable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Overlayable), new FrameworkPropertyMetadata(typeof(Overlayable)));
        }

        #region Child Content

        public static readonly DependencyProperty ChildProperty =
            DependencyProperty.Register("Child", typeof (object), typeof (Overlayable), new PropertyMetadata(null));

        public object Child
        {
            get { return GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ContentPresenter childPresenter = GetTemplateChild("PART_Child") as ContentPresenter;
            childPresenter?.SetBinding(ContentPresenter.ContentProperty, new Binding("Child") { Source = this });

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (Application.Current != null)
                    Application.Current.MainWindow.PreviewMouseDown += CloseMenu;
                PreviewMouseDown += OpenMenu;
            }
        }

        #endregion        

        public MenuModel Menu { get { return (MenuModel) GetValue(MenuProperty); } set { SetValue(MenuProperty, value); } }
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof (MenuModel), typeof (Overlayable));

        private readonly MediaMenu _mediaMenu;
        private PopUp _currentPopUp;

        public Overlayable()
        {
            _mediaMenu = new MediaMenu();
            _mediaMenu.SetBinding(MediaMenu.MenuProperty, new Binding("Menu") {Source = this});
        }

        private void CloseMenu(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            //TODO: Send To Window And Pop It up
            if (_currentPopUp != null && _mediaMenu.IsMouseOver == false)
            {
                Dispatch("Remove PopUp", _currentPopUp);
                _currentPopUp = null;
            }
        }

        private void OpenMenu(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            //TODO: Send To Window And Pop It up
            if (mouseButtonEventArgs.ChangedButton != MouseButton.Right) return;
            Dispatch("Add PopUp", _currentPopUp = new PopUp
            {
                PopUpElement = _mediaMenu,
                X = mouseButtonEventArgs.GetPosition(Application.Current.MainWindow).X,
                Y = mouseButtonEventArgs.GetPosition(Application.Current.MainWindow).Y
            });
        }

    }
}
