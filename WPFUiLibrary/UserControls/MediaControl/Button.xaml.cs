using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFUiLibrary.Utils;
using static DispatcherLibrary.Dispatcher;
using MediaState = MediaPropertiesLibrary.MediaState;

namespace WPFUiLibrary.UserControls.MediaControl
{
    /// <summary>
    /// Interaction logic for Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public Button()
        {
            InitializeComponent();
            PauseButton.Command = new UiCommand(o => Dispatch("Pause"));
            PlayButton.Command = new UiCommand(o =>
            {
                if (Status == MediaState.Paused)
                    Dispatch("Play");
                else if (LaunchCommand != null && LaunchCommand.CanExecute(LaunchCommandParameter))
                    LaunchCommand.Execute(LaunchCommandParameter);
            });
        }

        #region Custom Commands

        public ICommand LaunchCommand
        {
            get { return (ICommand) GetValue(LaunchCommandProperty); }
            set { SetValue(LaunchCommandProperty, value); }
        }

        public object LaunchCommandParameter
        {
            get { return GetValue(LaunchCommandParameterProperty); }
            set { SetValue(LaunchCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty LaunchCommandParameterProperty =
            DependencyProperty.Register("LaunchCommandParameter", typeof (object), typeof (Button));
        public static readonly DependencyProperty LaunchCommandProperty =
            DependencyProperty.Register("LaunchCommand", typeof (ICommand), typeof (Button));

        #endregion

        #region Display Status

        public bool Selected
        {
            get { return (bool) GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof (bool), typeof (Button), new PropertyMetadata
        {
            PropertyChangedCallback = (o, args) =>
            {
                Button button = o as Button;
                if (button == null || button.Status == MediaState.Playing) return;

                var value = (bool) args.NewValue;
                if (value)
                {
                    button.InPlayButton.Visibility = Visibility.Collapsed;
                    button.PauseButton.Visibility = Visibility.Collapsed;
                    button.PlayButton.Visibility = Visibility.Visible;
                    button.PlayButton.Opacity = 0.8;
                    button.Container.Visibility = Visibility.Visible;
                }
                else
                {
                    button.Container.Visibility = Visibility.Collapsed;
                }
            }
        });

        public MediaPropertiesLibrary.MediaState Status
        {
            get { return (MediaPropertiesLibrary.MediaState) GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof (MediaPropertiesLibrary.MediaState), typeof (Button), new PropertyMetadata
        {
            PropertyChangedCallback = (o, args) =>
            {
                Button button = o as Button;
                if (button == null) return;

                var value = (MediaPropertiesLibrary.MediaState) args.NewValue;
                if (value == MediaState.Playing)
                {
                    button.Container.Visibility = Visibility.Visible;
                    button.InPlayButton.Visibility = Visibility.Visible;
                    button.PauseButton.Visibility = Visibility.Collapsed;
                    button.PlayButton.Visibility = Visibility.Collapsed;
                }
                else if (button.Selected)
                {
                    button.InPlayButton.Visibility = Visibility.Collapsed;
                    button.PauseButton.Visibility = Visibility.Collapsed;
                    button.PlayButton.Visibility = Visibility.Visible;
                    button.PlayButton.Opacity = 0.8;
                }
                else
                {
                    button.InPlayButton.Visibility = Visibility.Collapsed;
                    button.PauseButton.Visibility = Visibility.Collapsed;
                    button.PauseButton.Visibility = Visibility.Collapsed;
                }
            }
        });

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Status == MediaState.Playing)
            {
                InPlayButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Visible;
            }
            else
            {
                InPlayButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                PlayButton.Opacity = 1.0;
            }
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Status == MediaState.Playing)
            {
                InPlayButton.Visibility = Visibility.Visible;
                PauseButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                InPlayButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                PlayButton.Opacity = 0.8;
            }
        }

        #endregion
    }
}
