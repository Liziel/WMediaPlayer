using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedDispatcher
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHook : Attribute
    {
        public EventHook(string hook)
        {
            Hook = hook;
        }

        internal string Hook { get; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ForwardDispatch : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CollectionForwardDispatch : Attribute
    {
    }

    public class Listener
    {
        private readonly Dictionary<string, MethodInfo>      _hookDictionary = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, List<FieldInfo>> _hookedForwards = new Dictionary<string, List<FieldInfo>>();

        protected Listener()
        {
            foreach (var methodInfo in from methodInfo in GetType().GetMethods()
                let attr = methodInfo.GetCustomAttributes(typeof (EventHook), true)
                where attr.Length > 0
                select methodInfo)
                foreach (EventHook hook in methodInfo.GetCustomAttributes(typeof (EventHook), true))
                    _hookDictionary[hook.Hook] = methodInfo;


            foreach (var attributeInfo in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                {
                    var attr = attributeInfo.GetCustomAttributes(typeof (ForwardDispatch), true);
                    if (attr.Length <= 0) continue;
                    foreach (var pair in ((Listener) attributeInfo.GetValue(this))._hookDictionary)
                    {
                        if (!_hookedForwards.ContainsKey(pair.Key))
                            _hookedForwards.Add(pair.Key, new List<FieldInfo>());
                        _hookedForwards[pair.Key].Add(attributeInfo);
                    }
                    
                }
            }
        }

        internal void DispatchInternal(string _event, object[] fwdparams)
        {
            if (_hookDictionary.ContainsKey(_event))
                _hookDictionary[_event].Invoke(this, fwdparams);
        }
    }
}