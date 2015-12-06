using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using SharedProperties.Customization;

namespace SharedProperties
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

        [ImportMany(typeof (IPlugin), AllowRecomposition = true)]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private List<Lazy<IPlugin>> ListPlugins { get; set; }

        public void LoadPlugin(string path, string pattern)
        {
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, pattern);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AttributedModelServices.ComposeParts(new CompositionContainer(directoryCatalog), this);
        }

        #endregion

        #region Query Methods

        public IPlugin SingleQuery(Predicate<IPlugin> predicate)
        {
            return ListPlugins.Find(plugin => predicate(plugin.Value))?.Value;
        }

        public IEnumerable<IPlugin> Query(Predicate<IPlugin> predicate)
        {
            return from plugin in ListPlugins
                where predicate(plugin.Value)
                select plugin.Value;
        }

        #endregion
    }
}