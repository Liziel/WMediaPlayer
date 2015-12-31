using System;
using System.Collections.Generic;

namespace DispatcherLibrary
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
        /// Instance for a singleton pattern
        /// </summary>
        private static readonly Dispatcher Instance = new Dispatcher();

        public static void AddEventListener(Listener listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            Instance._eventsConsummers.Add(listener);
        }

        public static void RemoveEnventListener(Listener listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));
            Instance._eventsConsummers.Remove(listener);
        }

        public static void Dispatch(string _event, params object[] fwdparams)
        {
            foreach (var consumer in Instance._eventsConsummers)
                consumer.DispatchInternal(_event, fwdparams);
        }

        public static void Dispatch(Listener target, string _event, params object[] fwdparams)
        {
            target.DispatchInternal(_event, fwdparams);
        }

    }
}