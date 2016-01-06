using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DispatcherLibrary
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

    [AttributeUsage(AttributeTargets.Method)]
    public class RequestHook : Attribute
    {
        public RequestHook(string hook)
        {
            Hook = hook;
        }

        internal string Hook { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ForwardDispatch : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionForwardDispatch : Attribute
    {
    }

    public class Listener
    {
        private readonly Dictionary<string, Action<object[]>> _onRunHookDictionary = new Dictionary<string, Action<object[]>>(); 
        private readonly Dictionary<string, List<MethodInfo>> _hookDictionary = new Dictionary<string, List<MethodInfo>>();

        private readonly Dictionary<string, List<MethodInfo>> _requestDictionary = new Dictionary<string, List<MethodInfo>>(); 

        private readonly List<PropertyInfo> _forwards;
        private readonly List<PropertyInfo> _forwardsCollection;

        protected Listener()
        {
            foreach (var methodInfo in from methodInfo in GetType().GetMethods()
                let attr = methodInfo.GetCustomAttributes(typeof (EventHook), true)
                where attr.Length > 0
                select methodInfo)
                foreach (EventHook hook in methodInfo.GetCustomAttributes(typeof (EventHook), true))
                {
                    if (!_hookDictionary.ContainsKey(hook.Hook))
                    {
                        _hookDictionary[hook.Hook] = new List<MethodInfo>();
                    }
                    _hookDictionary[hook.Hook].Add(methodInfo);
                }

            foreach (var methodInfo in from methodInfo in GetType().GetMethods()
                                       let attr = methodInfo.GetCustomAttributes(typeof(RequestHook), true)
                                       where attr.Length > 0
                                       select methodInfo)
                foreach (RequestHook hook in methodInfo.GetCustomAttributes(typeof(RequestHook), true))
                {
                    if (!_requestDictionary.ContainsKey(hook.Hook))
                    {
                        _requestDictionary[hook.Hook] = new List<MethodInfo>();
                    }
                    _requestDictionary[hook.Hook].Add(methodInfo);
                }

            _forwards = (from propertyInfo in GetType().GetProperties()
                let attr = propertyInfo.GetCustomAttributes(typeof (ForwardDispatch), true)
                where attr.Length > 0
                select propertyInfo).ToList();

            _forwardsCollection = (from propertyInfo in GetType().GetProperties()
                let attr = propertyInfo.GetCustomAttributes(typeof (CollectionForwardDispatch), true)
                where attr.Length > 0
                select propertyInfo).ToList();
        }

        private void DispatchToMembers(object target, string _event, object[] fwdparams)
        {
            if (target == null)
                return;
            var listener = target as Listener;
            listener?.DispatchInternal(_event, fwdparams);
        }

        internal void DispatchInternal(string _event, object[] fwdparams)
        {
            if (_hookDictionary.ContainsKey(_event))
                foreach (var methodInfo in _hookDictionary[_event])
                    try
                    {methodInfo.Invoke(this, fwdparams);}
                    catch (Exception e)
                    {/* ignored */}
            if (_onRunHookDictionary.ContainsKey(_event))
                _onRunHookDictionary[_event](fwdparams);
            foreach (var propertyInfo in _forwards)
                DispatchToMembers(propertyInfo.GetValue(this), _event, fwdparams);
            foreach (var forwards in _forwardsCollection.SelectMany(propertyInfo => (IEnumerable<object>) propertyInfo.GetValue(this)))
                DispatchToMembers(forwards, _event, fwdparams);
        }

        private IEnumerable<object> RequestToMembers(object target, string _event, object[] fwdparams)
        {
            var listener = target as Listener;
            if (listener == null) return  new List<object>();
            return listener.RequestInternal(_event, fwdparams);
        }

        internal IEnumerable<object> RequestInternal(string _event, object[] fwdparams)
        {
            List< object > results = new List<object>();
            if (_requestDictionary.ContainsKey(_event))
                foreach (var methodInfo in _requestDictionary[_event])
                    try
                    { results.Add(methodInfo.Invoke(this, fwdparams)); }
                    catch (Exception e)
                    {/* ignored */}
            foreach (var propertyInfo in _forwards)
                results.AddRange(RequestToMembers(propertyInfo.GetValue(this), _event, fwdparams));
            foreach (var forwards in _forwardsCollection.SelectMany(propertyInfo => (IEnumerable<object>)propertyInfo.GetValue(this)))
                results.AddRange(RequestToMembers(forwards, _event, fwdparams));
            return results;
        }

        protected void AttachOnRun(string _event, Action<object[]> action)
        {
            _onRunHookDictionary.Add(_event, action);
        }
    }
}