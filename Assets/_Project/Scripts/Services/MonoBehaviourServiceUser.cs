using UnityEngine;

namespace ColourMatch
{
    public abstract class MonoBehaviourServiceUser : MonoBehaviour
    {
        protected T ResolveServiceDependency<T>() where T : class
        {
            var service = ServiceLocator.TryGetService<T>();
            if (service == null)
            {
                var message = $"[ServiceUser] Required service not found: {typeof(T).FullName}";
                Logger.Error(this, message, LogChannel.Services);
                throw new System.InvalidOperationException(message);
            }

            return service;
        }
    }
}