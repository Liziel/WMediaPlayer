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
        /// EventsConsummers is the map where are stocked all event listeners
        /// </summary>
        internal List<Consumer> EventsConsummers = new List<Consumer>();


        /// <summary>
        /// _instance is a singleton pattern
        /// </summary>
        public static Dispatcher GetInstance => _instance ?? (_instance = new Dispatcher());

        private static Dispatcher _instance;

        public void AddEventListener(Consumer consumer)
        {
            if (consumer == null) throw new ArgumentNullException(nameof(consumer));
            EventsConsummers.Add(consumer);
        }

        public void RemoveEnventListener(Consumer consumer)
        {
            if (consumer == null) throw new ArgumentNullException(nameof(consumer));
            EventsConsummers.Remove(consumer);
        }

        public void Dispatch(Dispatchable dispatchable)
        {
            if (dispatchable == null) throw new ArgumentNullException(nameof(dispatchable));
            foreach (var consumer in EventsConsummers)
                (from methods in consumer.GetType().GetMethods()
                    where
                        methods.Name == "On" + dispatchable.Name &&
                        methods.GetParameters().Length == dispatchable.ParamsList.Length
                    select methods)?.Single().Invoke(consumer, dispatchable.ParamsList);
        }
    }
}