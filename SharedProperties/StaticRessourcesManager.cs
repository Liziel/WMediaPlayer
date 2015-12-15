using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using PluginLibrary.Customization;

namespace PluginLibrary
{
    public class StaticRessourcesManager : AbstractPluginManager
    {
        [ImportMany(typeof(IStaticRessource))]
        private IEnumerable<Lazy<IStaticRessource>> _importedPlugins;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly List<IStaticRessource> _listPlugins = new List<IStaticRessource>();

        private static readonly StaticRessourcesManager Instance = new StaticRessourcesManager();
        private StaticRessourcesManager() { }

        protected sealed override void OnPluginComposed()
        {
            foreach (var importedPlugin in Instance._importedPlugins)
                Instance._listPlugins.Add(importedPlugin.Value);
        }

        public static void StaticRessourcesInitialization()
        {
            Instance.ForeceLoadPlugin();
        }
    }
}