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
    public class SidePluginLoaderViewModel : Listener, INotifyPropertyChanged
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public SidePluginLoaderViewModel()
        {
            PluginLoaders = ViewPluginManager.GetInstance.Query(plugin => plugin is ILoadablePlugin)
                .Select(plugin => new PluginLoader(plugin as ILoadablePlugin, this.OnPluginSelected))
                .ToList();
        }

        private void OnPluginSelected(PluginLoader pluginLoader)
        {
            foreach (var loader in PluginLoaders)
                loader.PluginSelected = false;
            pluginLoader.PluginSelected = true;
            Dispatcher.GetInstance.Dispatch("Force Attach viewPlugin", pluginLoader.ViewPlugin, Position.Center,
                pluginLoader.ViewPlugin.Layer);
        }
    }
}