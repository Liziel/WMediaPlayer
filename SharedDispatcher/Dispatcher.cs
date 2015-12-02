using System;
using System.Collections.Generic;
using System.Linq;

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
        private List<Consumer> _eventsConsummers = new List<Consumer>();


        /// <summary>
        /// _instance is a singleton pattern
        /// </summary>
        public static Dispatcher GetInstance => _instance ?? (_instance = new Dispatcher());

        private static Dispatcher _instance;

        public void AddEventListener(Consumer consumer)
        {
            if (consumer == null) throw new ArgumentNullException(nameof(consumer));
            _eventsConsummers.Add(consumer);
        }

        public void RemoveEnventListener(Consumer consumer)
        {
            if (consumer == null) throw new ArgumentNullException(nameof(consumer));
            _eventsConsummers.Remove(consumer);
        }

        public void Dispatch(string _event, params object[] fwdparams)
        {
            foreach (var consumer in _eventsConsummers)
                if (consumer.Forward == null)
                    consumer.GetType().GetMethod("On" + _event)?.Invoke(consumer, fwdparams);
                else
                    consumer.Forward.GetType().GetMethod("On" + _event)?.Invoke(consumer.Forward, fwdparams);
        }

        public void Dispatch(Consumer consumer, string _event, params object[] fwdparams)
        {
            if (consumer.Forward == null)
                consumer.GetType().GetMethod("On" + _event)?.Invoke(consumer, fwdparams);
            else
                consumer.Forward.GetType().GetMethod("On" + _event)?.Invoke(consumer.Forward, fwdparams);
        }
    }
}