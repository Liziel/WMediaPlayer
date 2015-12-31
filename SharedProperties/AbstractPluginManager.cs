using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;

namespace PluginLibrary
{
    public abstract class AbstractPluginManager
    {
        private bool _loaded = false;

        protected abstract void OnPluginComposed();

        private void BaseLoadPlugin()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            var pattern = "*.dll";
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, pattern);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AttributedModelServices.ComposeParts(new CompositionContainer(directoryCatalog), this);
            OnPluginComposed();
            _loaded = true;
        }

        public void ForeceLoadPlugin() { BaseLoadPlugin(); }


        protected TPluginType BaseSingleQuery<TPluginType>(Predicate<TPluginType> predicate, List<TPluginType> list)
        {
            if (!_loaded) BaseLoadPlugin();
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return list.Find(predicate);
        }

        protected IEnumerable<TPluginType> BaseQuery<TPluginType>(Predicate<TPluginType> predicate, List<TPluginType> list)
        {
            if (!_loaded) BaseLoadPlugin();
            if (predicate == null) throw new ArgumentNullException();
            return from plugin in list
                where predicate(plugin)
                select plugin;
        }
    }
}