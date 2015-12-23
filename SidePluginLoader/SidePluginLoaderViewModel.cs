using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using DispatcherLibrary;
using PluginLibrary;
using PluginLibrary.Customization;
using SidePluginLoader.Annotations;

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
            Dispatcher.GetInstance.AddEventListener(this);
            PluginLoaders = LoadablePluginManager.GetInstance.Query(plugin => true)
                .Select(plugin => new PluginLoader(plugin, OnPluginSelected))
                .ToList();
            foreach (var loader in PluginLoaders)
                AttachOnRun("Loader: Call(" + loader.PluginName + ")", objects => OnPluginSelected(loader));
        }

        private void OnPluginSelected(PluginLoader pluginLoader)
        {
            foreach (var loader in PluginLoaders)
                loader.PluginSelected = false;
            pluginLoader.PluginSelected = true;
            Dispatcher.GetInstance.Dispatch("Attach Plugin", pluginLoader.ViewPlugin);
        }

        [EventHook("Force Load Plugin")]
        public void OnForceLoadPlugin(string pluginName)
        {
            var pluginLoader = PluginLoaders.First(plugin => plugin.PluginName == pluginName);

            foreach (var loader in PluginLoaders)
                loader.PluginSelected = false;
            pluginLoader.PluginSelected = true;
            Dispatcher.GetInstance.Dispatch("Attach Plugin", pluginLoader.ViewPlugin);
        }
    }
}