using UnityEngine;

namespace ColourMatch
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolableParticleVFX : MonoBehaviour, IPoolable
    {
        private ParticleSystem _particleSystem;
        private PoolManager _poolManager;
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

        public void Configure(PoolManager poolManager, PooledObject pooledObject)
        {
            _poolManager = poolManager;
            _pooledObject = pooledObject;
            isConfigured = true;
        }

        public void OnSpawned()
        {
            if (!isConfigured)
            {
                Debug.LogWarning($"[PoolableParticleVFX] VFX not configured properly. Assign PoolManager and tag.");
                return;
            }
            
            Debug.Log($"VFX has spawned");

            CancelInvoke();
            InvokeRepeating(nameof(CheckIfFinished), 0.2f, 0.1f);
        }

        public void OnReturned()
        {
            CancelInvoke();
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Debug.Log($"VFX has been returned.");
        }

        private void CheckIfFinished()
        {
            if (_particleSystem == null || _particleSystem.IsAlive()) return;

            CancelInvoke();
            if (_poolManager != null)
            {
                _poolManager.Return(_pooledObject, gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}