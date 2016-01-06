using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using WPFUiLibrary.Utils;

namespace MyWindowsMediaPlayerv2
{
    /// <summary>
    /// Logique d'interaction pour MediaControlView.xaml
    /// </summary>
    public partial class MediaControlView : Window
    {
        public MediaControlView()
        {
            InitializeComponent();
            CloseButton.Command = new UiCommand(o => SystemCommands.CloseWindow(GetWindow(this)));
            Minimize.Command = new UiCommand(o => SystemCommands.MinimizeWindow(GetWindow(this)));
            RestoreWindow.Command = new UiCommand(o => SystemCommands.RestoreWindow(this));
            MaximizeWindow.Command = new UiCommand(o => SystemCommands.MaximizeWindow(this));
        }
    }
}
