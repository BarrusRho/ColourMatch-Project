using UnityEngine;

namespace ColourMatch
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolableParticleVFX : MonoBehaviour, IPoolable
    {
        private ParticleSystem _particleSystem;
        private PoolingService _poolingService;
        private PooledObject _pooledObject;

        private bool isConfigured;
        public bool IsConfigured => isConfigured;

        private void Awake()
        {
            if (_particleSystem == null)
            {
                _particleSystem = GetComponent<ParticleSystem>();
            }
        }

        public void Configure(PoolingService poolingService, PooledObject pooledObject)
        {
            _poolingService = poolingService;
            _pooledObject = pooledObject;
            isConfigured = true;
        }

        public void OnSpawned()
        {
            if (!isConfigured)
            {
                Logger.Warning(this, $"[PoolableParticleVFX] VFX not configured properly. Assign PoolManager and tag.", LogChannel.PoolingService);
                return;
            }
            
            Logger.BasicLog(this, $"VFX has spawned", LogChannel.PoolingService);

            CancelInvoke();
            InvokeRepeating(nameof(CheckIfFinished), 0.2f, 0.1f);
        }

        public void OnReturned()
        {
            CancelInvoke();
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Logger.BasicLog(this, $"VFX has been returned", LogChannel.PoolingService);
        }

        private void CheckIfFinished()
        {
            if (_particleSystem == null || _particleSystem.IsAlive()) return;

            CancelInvoke();
            if (_poolingService != null)
            {
                _poolingService.Return(_pooledObject, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}