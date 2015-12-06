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
using SharedDispatcher;
using SharedProperties;
using SharedProperties.Customization;

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
            public void AttachPlugin(IPlugin plugin) { _viewAnchorer.AttachPlugin(plugin);}
            [EventHook("Force Attach Plugin")]
            public void ForceAttachPlugin(IPlugin plugin, Position position, int layer) { _viewAnchorer.ForceAttachPlugin(plugin, position, layer); }

            #endregion

            [CollectionForwardDispatch]
            public List<IPlugin> NonOptionalPlugins => _nonOptionalPlugins;
            private List<IPlugin> _nonOptionalPlugins;

            #region Wpf Visibility

            private string _windowState = "normal";

            public string WindowState
            {
                get { return _windowState; }
                set
                {
                    _windowState = value;
                    OnPropertyChanged(nameof(WindowState));
                }
            }

            private string _windowStyle = "SingleBorderWindow";

            public string WindowStyle
            {
                get { return _windowStyle; }
                set
                {
                    _windowStyle = value;
                    OnPropertyChanged(nameof(WindowStyle));
                }
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
                PluginManager.GetInstance.LoadPlugin(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");

                RootElement = _viewAnchorer.RootElement;
                foreach (
                    var plugin in
                        PluginManager.GetInstance.Query(
                            plugin => plugin.Optional == false && plugin.Position != Position.Invisible))
                    _viewAnchorer.AttachPlugin(plugin);
                _nonOptionalPlugins = PluginManager.GetInstance.Query(plugin => plugin.Optional == false).ToList();

                Dispatcher.GetInstance.AddEventListener(this);
                Dispatcher.GetInstance.Dispatch("Media Opening",
                    new Uri(@"C:\Users\Colliot Vincent\Music\Hiroyuki Sawano\KILL la KILL ORIGINAL SOUND TRACK\01 澤野 弘之 - Before my body is dry.mp3"));

            }

            #endregion

        }
    }
}
