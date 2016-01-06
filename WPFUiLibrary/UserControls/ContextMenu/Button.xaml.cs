using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFUiLibrary.UserControls.PopupManager;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;

namespace WPFUiLibrary.UserControls.ContextMenu
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public Button()
        {
            InitializeComponent();
            Menu = new MenuModel();
            if (Template == null)
                Template = (ControlTemplate)FindResource("ButtonTemplate");
            _mediaMenu.SetBinding(MediaMenu.MenuProperty, new Binding("Menu") {Source = this});
            MenuButton.SetBinding(System.Windows.Controls.Button.TemplateProperty,
                new Binding("Template") {Source = this});
            if (!DesignerProperties.GetIsInDesignMode(this))
                Application.Current.MainWindow.PreviewMouseDown += delegate(object sender, MouseButtonEventArgs args)
                    {
                        if (_mediaMenu.IsMouseOver == false && _currentPopUp != null)
                        {
                            Dispatch("Remove PopUp", _currentPopUp);
                            Menu?.Reset();
                            _currentPopUp = null;
                        }
                    };
        }

        private readonly MediaMenu _mediaMenu = new MediaMenu();
        private PopUp _currentPopUp = null;

        public MenuModel Menu { get { return (MenuModel) GetValue(MenuProperty); } set {SetValue(MenuProperty, value);} }
        public static readonly DependencyProperty MenuProperty =
            DependencyProperty.Register("Menu", typeof(MenuModel), typeof(Button));

        public ControlTemplate Template { get { return (ControlTemplate) GetValue(TemplateProperty); } set {SetValue(TemplateProperty, value);} }
        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(Button));

        private void MenuDisplay(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.ChangedButton != MouseButton.Left) return;
            Dispatch("Add PopUp", _currentPopUp = new PopUp
            {
                PopUpElement = _mediaMenu,
                X = mouseButtonEventArgs.GetPosition(Application.Current.MainWindow).X,
                Y = mouseButtonEventArgs.GetPosition(Application.Current.MainWindow).Y
            });

        }
    }
}
