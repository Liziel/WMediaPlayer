using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PluginLibrary.Customization;

namespace PluginLibrary
{
    public class LoadablePluginManager : AbstractPluginManager
    {
        #region Singleton Properties and Constructor

        private static LoadablePluginManager _instance;

        public static LoadablePluginManager GetInstance
        {
            get
            {
                if (_instance == null) _instance = new LoadablePluginManager();
                return _instance;
            }
        }

        private LoadablePluginManager()
        {
        }

        #endregion

        #region Plugins List

        [ImportMany(typeof(ILoadablePlugin))]
        private IEnumerable<Lazy<ILoadablePlugin>> _importedPlugins;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly List<ILoadablePlugin> _listPlugins = new List<ILoadablePlugin>(); //TODO: ça merde saloooooope

        protected sealed override void OnPluginComposed()
        {
            foreach (var importedPlugin in _importedPlugins)
                _listPlugins.Add(importedPlugin.Value);
        }

        #endregion

        #region Query Methods

        public ILoadablePlugin SingleQuery(Predicate<ILoadablePlugin> predicate)
        {
            return BaseSingleQuery(predicate, _listPlugins);
        }

        public IEnumerable<ILoadablePlugin> Query(Predicate<ILoadablePlugin> predicate)
        {
            return BaseQuery(predicate, _listPlugins);
        }

        #endregion

    }
}