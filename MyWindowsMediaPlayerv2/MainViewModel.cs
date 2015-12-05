using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using MyWindowsMediaPlayerv2.Annotations;
using SharedDispatcher;
using SharedProperties.Customization;

namespace MyWindowsMediaPlayerv2
{
    namespace ViewModel
    {
        class MainViewModel : Listener, INotifyPropertyChanged
        {
            #region Models
            private PluginManager _pluginManager = new PluginManager();
            #endregion

            #region WPF Plugins

            private IPlugin _rightView;
            [ForwardDispatch]
            public IPlugin RightView { get { return _rightView; } set { _rightView = value; OnPropertyChanged(nameof(RightView)); } }

            private IPlugin _leftView;
            [ForwardDispatch]
            public IPlugin LeftView { get { return _leftView; } set { _leftView = value; OnPropertyChanged(nameof(LeftView)); } }

            private IPlugin _topView;
            [ForwardDispatch]
            public IPlugin TopView { get { return _topView; } set { _topView = value; OnPropertyChanged(nameof(TopView)); } }

            private IPlugin _bottomView;
            [ForwardDispatch]
            public IPlugin BottomView { get { return _bottomView; } set { _bottomView = value; OnPropertyChanged(nameof(BottomView)); } }

            private IPlugin _centerView;
            [ForwardDispatch]
            public IPlugin CenterView { get { return _centerView; } set { _centerView = value; OnPropertyChanged(nameof(CenterView)); } }

            [CollectionForwardDispatch]
            public List<IPlugin> HiddenPlugins { get; } = new List<IPlugin>();

            #endregion

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
                _pluginManager.LoadPlugin(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");

                CenterView = _pluginManager.SingleQuery(Position.Center, false);
                RightView = _pluginManager.SingleQuery(Position.Right, false);
                LeftView = _pluginManager.SingleQuery(Position.Left, false);
                BottomView = _pluginManager.SingleQuery(Position.Bottom, false);
                TopView = _pluginManager.SingleQuery(Position.Top, false);

                HiddenPlugins = _pluginManager.Query(Position.Invisible, false).ToList();

                Dispatcher.GetInstance.AddEventListener(this);
                Dispatcher.GetInstance.Dispatch("Media Opening",
                    new Uri("C:\\Users\\Colliot Vincent\\Music\\Begin Again OST\\a.mp3"));

            }

            #endregion

        }
    }
}
