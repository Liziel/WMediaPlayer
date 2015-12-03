using System;
using System.Collections.Generic;

namespace SharedDispatcher
{
    public class Dispatcher
    {
        private Dispatcher()
        {
        }

        /// <summary>
        /// _eventsConsummers is the map where are stocked all event listeners
        /// </summary>
        private readonly List<Listener> _eventsConsummers = new List<Listener>();


        /// <summary>
        /// _instance for a singleton pattern
        /// </summary>
        public static Dispatcher GetInstance => _instance ?? (_instance = new Dispatcher());
        private static Dispatcher _instance;

        internal void AddEventListener(Listener listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            _eventsConsummers.Add(listener);
        }
        internal void RemoveEnventListener(Listener listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            _eventsConsummers.Remove(listener);
        }

        public void Dispatch(string _event, params object[] fwdparams)
        {
            foreach (var consumer in _eventsConsummers)
                consumer.DispatchInternal(_event, fwdparams);
        }

        public void Dispatch(Listener target, string _event, params object[] fwdparams)
        {
            target.DispatchInternal(_event, fwdparams);
        }
    }
}