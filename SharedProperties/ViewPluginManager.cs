using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using PluginLibrary.Customization;

namespace PluginLibrary
{
    public class ViewPluginManager : AbstractPluginManager
    {
        #region Singleton Properties and Constructor

        private static ViewPluginManager _instance;

        public static ViewPluginManager GetInstance
        {
            get
            {
                if (_instance == null) _instance = new ViewPluginManager();
                return _instance;
            }
        }

        private ViewPluginManager()
        {
        }

        #endregion

        #region Plugins List

        [ImportMany(typeof (IViewPlugin))] private IEnumerable<Lazy<IViewPlugin>> _importedPlugins;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly List<IViewPlugin> _listPlugins = new List<IViewPlugin>();

        public void LoadPlugin(string path, string pattern)
        {
            BaseLoadPlugin(path, pattern);
            foreach (var importedPlugin in _importedPlugins)
                _listPlugins.Add(importedPlugin.Value);
        }

        #endregion

        #region Query Methods

        public IViewPlugin SingleQuery(Predicate<IViewPlugin> predicate)
        {
            return BaseSingleQuery(predicate, _listPlugins);
        }

        public IEnumerable<IViewPlugin> Query(Predicate<IViewPlugin> predicate)
        {
            return BaseQuery(predicate, _listPlugins);
        }

        #endregion
    }
}