using System;
using System.Collections.Generic;

namespace ColourMatch
{
    public class GameplayControllerService
    {
        private readonly Dictionary<Type, IGameplayController> controllers = new();
        
        public void Initialise()
        {
            
        }
        
        public void Dispose()
        {
            Clear();
            Logger.BasicLog(this, "Disposed GameplayControllerService and its controllers.", LogChannel.GameplayControllerService);
        }

        /// <summary>
        /// Register a gameplay controller. Throws if duplicate type is registered.
        /// </summary>
        public void Register<T>(T controller) where T : IGameplayController
        {
            var type = typeof(T);
            if (controllers.ContainsKey(type))
            {
                Logger.Warning(this, $"GameplayController of type {type.Name} is already registered.", LogChannel.GameplayControllerService);
                return;
            }

            controllers[type] = controller;
            Logger.BasicLog(this, $"Registered GameplayController: {type.Name}", LogChannel.GameplayControllerService);
        }
        
        public bool TryRegister<T>(T controller) where T : IGameplayController
        {
            var type = typeof(T);
            if (controllers.ContainsKey(type)) return false;
    
            controllers[type] = controller;
            Logger.BasicLog(this, $"Registered GameplayController: {type.Name}", LogChannel.GameplayControllerService);
            return true;
        }

        /// <summary>
        /// Get a registered gameplay controller of type T.
        /// </summary>
        public T Get<T>() where T : class, IGameplayController
        {
            var type = typeof(T);
            if (controllers.TryGetValue(type, out var controller))
            {
                return controller as T;
            }

            Logger.Error(this, $"GameplayController of type {type.Name} is not registered.", LogChannel.GameplayControllerService);
            throw new InvalidOperationException($"GameplayController {type.Name} not found.");
        }
        
        public bool TryGet<T>(out T controller) where T : class, IGameplayController
        {
            if (controllers.TryGetValue(typeof(T), out var result) && result is T casted)
            {
                controller = casted;
                return true;
            }

            controller = null;
            return false;
        }
        
        /// <summary>
        /// Check if a gameplay controller of type T is registered.
        /// </summary>
        public bool Contains<T>() where T : IGameplayController
        {
            return controllers.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Unregister a gameplay controller of type T.
        /// </summary>
        public bool Unregister<T>() where T : IGameplayController
        {
            var type = typeof(T);
            if (controllers.Remove(type))
            {
                Logger.BasicLog(this, $"Unregistered GameplayController: {type.Name}", LogChannel.GameplayControllerService);
                return true;
            }

            Logger.Warning(this, $"Attempted to unregister non-existent GameplayController: {type.Name}", LogChannel.GameplayControllerService);
            return false;
        }

        /// <summary>
        /// Resets all registered gameplay controllers (if they implement a Reset method).
        /// </summary>
        public void ResetAll()
        {
            foreach (var controller in controllers.Values)
            {
                if (controller is IResettable resettable)
                {
                    resettable.Reset();
                    Logger.BasicLog(this, $"Reset controller: {controller.GetType().Name}", LogChannel.GameplayControllerService);
                }
            }
        }

        /// <summary>
        /// Clear all registered gameplay controllers.
        /// </summary>
        public void Clear()
        {
            if (controllers.Count == 0) return;
            
            foreach (var controller in controllers.Values)
            {
                if (controller is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            controllers.Clear();
            Logger.BasicLog(this, "Cleared all gameplay controllers.", LogChannel.GameplayControllerService);
        }
    }
}

