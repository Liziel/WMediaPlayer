using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using PluginLibrary.Customization;

namespace PluginLibrary
{
    public class PluginManager
    {
        #region Singleton Properties and Constructor

        static private PluginManager _instance;

        static public PluginManager GetInstance
        {
            get
            {
                if (_instance == null) _instance = new PluginManager();
                return _instance;
            }
        }

        private PluginManager()
        {
        }

        #endregion

        #region Plugins List

        [ImportMany(typeof (IPlugin))] private IEnumerable<Lazy<IPlugin>> _importedPlugins;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly List<IPlugin> _listPlugins = new List<IPlugin>();

        public void LoadPlugin(string path, string pattern)
        {
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, pattern);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AttributedModelServices.ComposeParts(new CompositionContainer(directoryCatalog), this);
            foreach (var importedPlugin in _importedPlugins)
                _listPlugins.Add(importedPlugin.Value);
        }

        #endregion

        #region Query Methods

        public IPlugin SingleQuery(Predicate<IPlugin> predicate)
        {
            return _listPlugins.Find(plugin => predicate(plugin));
        }

        public IEnumerable<IPlugin> Query(Predicate<IPlugin> predicate)
        {
            return from plugin in _listPlugins
                where predicate(plugin)
                select plugin;
        }

        #endregion
    }
}