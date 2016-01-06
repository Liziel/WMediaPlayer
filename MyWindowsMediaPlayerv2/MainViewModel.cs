using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using MyWindowsMediaPlayerv2.Annotations;
using DispatcherLibrary;
using PluginLibrary;
using PluginLibrary.Customization;
using WPFUiLibrary.Utils;

namespace MyWindowsMediaPlayerv2
{
    namespace ViewModel
    {
        class MainViewModel : Listener, INotifyPropertyChanged
        {
            #region Anchor Model

            private readonly ViewAnchorer _viewAnchorer = new ViewAnchorer();

            private UIElement _rootElement;

            public UIElement RootElement
            {
                get { return _rootElement; }
                set
                {
                    _rootElement = value;
                    OnPropertyChanged(nameof(RootElement));
                }
            }

            [EventHook("Attach Plugin")]
            public void AttachPlugin(IViewPlugin viewPlugin)
            {
                _viewAnchorer.AttachPlugin(viewPlugin);
            }

            [EventHook("Attach Plugin On Top")]
            public void PutPluginOnTop(IViewPlugin viewPlugin)
            {
                _viewAnchorer.PutPluginOnTop(viewPlugin);
            }

            [EventHook("Remove Plugin")]
            public void RemovePlugin(IViewPlugin viewPlugin)
            {
                _viewAnchorer.DesattachPlugin(viewPlugin);
            }

            #endregion

            #region Non Optional Plugins Event Forwarding

            [CollectionForwardDispatch]
            public List<IMessageablePlugin> MessageablePlugins { get; }

            private List<ILoadablePlugin> LoadablePlugins { get; }

            #endregion

            #region Wpf Visibility

            private WindowState _lastState;
            private WindowState _state = WindowState.Maximized;
            public WindowState FullScreenState
            {
                get { return _state; }
                set
                {
                    _state = value;
                    OnPropertyChanged(nameof(FullScreenState));
                }
            }

            private WindowStyle _fullScreenStyle = WindowStyle.SingleBorderWindow;
            public WindowStyle FullScreenStyle
            {
                get { return _fullScreenStyle; }
                set
                {
                    _fullScreenStyle = value;
                    OnPropertyChanged(nameof(FullScreenStyle));
                }
            }

            [EventHook("View: Fullscreen")]
            public void EnterFullScreen()
            {
                FullScreenStyle = WindowStyle.None;
                _lastState = _state;
                FullScreenState = WindowState.Normal;
                FullScreenState = WindowState.Maximized;
            }

            [EventHook("View: Normal")]
            public void ExitFullScreen()
            {
                FullScreenStyle = WindowStyle.SingleBorderWindow;
                FullScreenState = _lastState;
            }

            #endregion

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion

            #region Constructor

            public MainViewModel()
            {
                StaticRessourcesManager.StaticRessourcesInitialization();

                RootElement = _viewAnchorer.RootElement;
                foreach (
                    var plugin in
                        ViewPluginManager.GetInstance.Query(
                            plugin => plugin.Optional == false && plugin.Position != Position.Invisible))
                    _viewAnchorer.AttachPlugin(plugin);
                MessageablePlugins =
                    MessageablePluginManager.GetInstance.Query(plugin => plugin.Optional == false).ToList();
                LoadablePlugins = LoadablePluginManager.GetInstance.Query(plugin => true).ToList();

                Dispatcher.AddEventListener(this);
            }

            #endregion

            [EventHook("Current Media Name")]
            public void CurrentMediaName(string mediaName) => WindowName = mediaName;

            [EventHook("Media Playing")]
            public void OnPlay() { TaskBarState = ETaskBarState.Play; }
            [EventHook("Media Paused")]
            public void OnPause() { TaskBarState = ETaskBarState.Pause; }
            public UiCommand Prev { get; } = new UiCommand(o => Dispatcher.Dispatch("Previous Track"));
            public UiCommand Next { get; } = new UiCommand(o => Dispatcher.Dispatch("Next Track"));
            private UiCommand Play { get; } = new UiCommand(o => Dispatcher.Dispatch("Play"));
            private UiCommand Pause { get; } = new UiCommand(o => Dispatcher.Dispatch("Pause"));
            public UiCommand PlayPause => TaskBarState == ETaskBarState.Play ? Pause : Play;
            public string PlayPauseIcon => TaskBarState == ETaskBarState.Play ? "TaskbarIcons/pause.png" : "TaskbarIcons/play.png";

            public enum ETaskBarState { Hide, Play, Pause }
            private ETaskBarState _eTaskBarState = ETaskBarState.Hide;

            public bool TaskBarHidded => TaskBarState == ETaskBarState.Hide;
            public ETaskBarState    TaskBarState
            {
                get { return _eTaskBarState; }
                set
                {
                    _eTaskBarState = value;
                    OnPropertyChanged(nameof(TaskBarState));
                    OnPropertyChanged(nameof(TaskBarHidded));

                    OnPropertyChanged(nameof(PlayPause));
                    OnPropertyChanged(nameof(PlayPauseIcon));
                }
            }

            private string _windowName = "GJVMediaPlayer";
            public string WindowName { get { return _windowName; } set { _windowName = value; OnPropertyChanged(nameof(WindowName)); } }
        }
    }
}
