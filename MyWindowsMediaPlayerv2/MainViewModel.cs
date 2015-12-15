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

            private bool _fullScreenState = false;

            public bool FullScreenState
            {
                get { return _fullScreenState; }
                set
                {
                    _fullScreenState = value;
                    OnPropertyChanged(nameof(FullScreenState));
                }
            }

            [EventHook("Enter Fullscreen")]
            public void EnterFullScreen()
            {
                FullScreenState = true;
            }

            [EventHook("Exit Fullscreen")]
            public void ExitFullScreen()
            {
                FullScreenState = false;
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

                Dispatcher.GetInstance.AddEventListener(this);
            }

            #endregion
        }
    }
}
