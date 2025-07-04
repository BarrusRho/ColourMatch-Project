using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ColourMatch
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
            {
                Debug.LogWarning($"Service of type {type.FullName} is already registered. Overwriting.");
            }
            _services[type] = service;
        }

        public static void RegisterOnce<T>(T service) where T : class
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                _services[type] = service;
            }
        }
        
        public static async Task RegisterAsync<T>(T service) where T : class
        {
            if (service is IAsyncInitialisable initialisable)
            {
                await initialisable.InitialiseAsync();
            }
            else
            {
                Debug.LogWarning($"RegisterAsync was called on {typeof(T).FullName}, but it doesn't implement IAsyncInitialisable.");
            }

            var type = typeof(T);
            _services[type] = service;
        }
        
        public static async Task RegisterAsyncOnce<T>(T service) where T : class
        {
            var type = typeof(T);
            if (!_services.ContainsKey(type))
            {
                if (service is IAsyncInitialisable initialisable)
                {
                    await initialisable.InitialiseAsync();
                }
                _services[type] = service;
            }
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
            {
                return service as T;
            }
            
            throw new InvalidOperationException($"Service of type {type.FullName} has not been registered.");
        }
        
        public static bool TryGet<T>(out T service) where T : class
        {
            if (_services.TryGetValue(typeof(T), out var serviceInstance))
            {
                service = serviceInstance as T;
                return true;
            }

            service = null;
            return false;
        }

        public static T TryGetService<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var serviceInstance))
            {
                return serviceInstance as T;
            }
            
            Debug.LogError($"[ServiceLocator] Service of type {typeof(T).FullName} has not been registered.");
            return null;
        }

        public static bool IsRegistered<T>() where T : class
        {
            return _services.ContainsKey(typeof(T));
        }

        public static void Clear()
        {
            _services.Clear();
        }
        
        public static void Unregister<T>() where T : class
        {
            var type = typeof(T);
            if (_services.Remove(type))
            {
                Logger.BasicLog(typeof(ServiceLocator), $"Unregistered service: {type.Name}", LogChannel.Services);
            }
            else
            {
                Logger.Warning(typeof(ServiceLocator), $"Attempted to unregister non-existent service: {type.Name}", LogChannel.Services);
            }
        }
    }
}