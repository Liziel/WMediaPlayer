using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary;
using PluginLibrary.Customization;
using SidePluginLoader.Annotations;
using static DispatcherLibrary.Dispatcher;

namespace SidePluginLoader
{
    public sealed class SidePluginLoaderViewModel : Listener, INotifyPropertyChanged
    {
        #region Loadable Plugins

        private List<PluginLoader> _pluginLoaders = null;

        public List<PluginLoader> PluginLoaders
        {
            get { return _pluginLoaders; }
            set
            {
                _pluginLoaders = value;
                OnPropertyChanged(nameof(PluginLoaders));
            }
        }

        #endregion

        #region INotify Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public SidePluginLoaderViewModel()
        {
            AddEventListener(this);
            PluginLoaders = LoadablePluginManager.GetInstance.Query(plugin => true)
                .Select(plugin => new PluginLoader(plugin, OnPluginSelected))
                .ToList();
            foreach (var loader in PluginLoaders)
                AttachOnRun("Loader: Call(" + loader.PluginName + ")", objects => OnPluginSelected(loader));
        }

        private CenterLoadableView _centerLoadableView;

        private object _view;
        public object View
        {
            get { return _view; }
            set
            {
                _view = value; 
                OnPropertyChanged(nameof(View));
            }
        }

        private void OnPluginSelected(PluginLoader pluginLoader)
        {
            if (_centerLoadableView == null)
                Dispatch("Attach Plugin", _centerLoadableView = new CenterLoadableView() { DataContext = this });
            foreach (var loader in PluginLoaders)
                loader.PluginSelected = false;
            pluginLoader.PluginSelected = true;
            View = pluginLoader.ViewPlugin;
        }

        [EventHook("Force Load Plugin")]
        public void OnForceLoadPlugin(string pluginName)
        {
            var pluginLoader = PluginLoaders.First(plugin => plugin.PluginName == pluginName);

            if (_centerLoadableView == null)
                Dispatch("Attach Plugin", _centerLoadableView = new CenterLoadableView() { DataContext = this });
            foreach (var loader in PluginLoaders)
                loader.PluginSelected = false;
            pluginLoader.PluginSelected = true;
            View = pluginLoader.ViewPlugin;
        }



        private ObservableCollection<UIElement> _pages = new ObservableCollection<UIElement>();
        public ObservableCollection<UIElement> Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                OnPropertyChanged(nameof(Pages));
            }
        }

        [EventHook("Publish pages")]
        public void PlublishPages(UIElement page)
        {
            Pages.Add(page);
        }
    }
}