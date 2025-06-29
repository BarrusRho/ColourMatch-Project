using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColourMatch
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _listeners = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_listeners.ContainsKey(type))
                _listeners[type] = new List<Delegate>();

            if (!_listeners[type].Contains(callback))
            {
                _listeners[type].Add(callback);
                Logger.BasicLog(typeof(EventBus), $"Subscribed to {type.Name}", LogChannel.Events);
            }
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (_listeners.TryGetValue(type, out var list) && list.Remove(callback))
            {
                Logger.BasicLog(typeof(EventBus), $"Unsubscribed from {type.Name}", LogChannel.Events);
            }
        }
        
        public static void Fire<T>(T evt)
        {
            if (_listeners.TryGetValue(typeof(T), out var list))
            {
                Logger.BasicLog(typeof(EventBus), $"Fired event: {typeof(T).Name}", LogChannel.Events);

                var copy = list.ToList();
                foreach (var callback in copy.Cast<Action<T>>())
                {
                    callback.Invoke(evt);
                }
            }
            else
            {
                Logger.Warning(typeof(EventBus), $"Fired event with no listeners: {typeof(T).Name}", LogChannel.Events);
            }
        }

        public static void Clear()
        {
            _listeners.Clear();
            Logger.BasicLog(typeof(EventBus), "All listeners cleared.", LogChannel.Events);
        }
    }
}