using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace PluginLibrary
{
    public class MessageablePluginManager : AbstractPluginManager
    {
        #region Singleton Properties and Constructor

        private static MessageablePluginManager _instance;

        public static MessageablePluginManager GetInstance
        {
            get
            {
                if (_instance == null) _instance = new MessageablePluginManager();
                return _instance;
            }
        }

        private MessageablePluginManager()
        {
        }

        #endregion

        #region Plugins List

        [ImportMany(typeof(IMessageablePlugin))]
        private IEnumerable<Lazy<IMessageablePlugin>> _importedPlugins;
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private readonly List<IMessageablePlugin> _listPlugins = new List<IMessageablePlugin>();

        public event MessageablePluginStatusChanged PluginStatusChanged;

        protected sealed override void OnPluginComposed()
        {
            foreach (var importedPlugin in _importedPlugins)
            {
                _listPlugins.Add(importedPlugin.Value);
                importedPlugin.Value.StatusChanged += delegate(object o) { OnPluginStatusChanged(this, importedPlugin.Value); };
            }
        }

        #endregion

        #region Query Methods

        public IMessageablePlugin SingleQuery(Predicate<IMessageablePlugin> predicate)
        {
            return BaseSingleQuery(predicate, _listPlugins);
        }

        public IEnumerable<IMessageablePlugin> Query(Predicate<IMessageablePlugin> predicate)
        {
            return BaseQuery(predicate, _listPlugins);
        }

        #endregion

        protected virtual void OnPluginStatusChanged(object source, IMessageablePlugin plugin)
        {
            PluginStatusChanged?.Invoke(source, plugin);
        }
    }
}