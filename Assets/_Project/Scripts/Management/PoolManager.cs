using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ColourMatch
{
    public class PoolManager
    {
        private readonly Dictionary<PooledObject, IObjectPool<GameObject>> _objectPools = new();

        public PoolManager(IEnumerable<ObjectPoolSO> pooledObjects)
        {
            foreach (var pooledObjectSO in pooledObjects)
            {
                if (pooledObjectSO.pooledObjectPrefab == null)
                {
                    Debug.LogWarning($"Skipping pool config with missing prefab for tag: {pooledObjectSO.pooledObject}");
                    continue;
                }

                if (_objectPools.ContainsKey(pooledObjectSO.pooledObject))
                {
                    Debug.LogWarning($"Duplicate pool tag: {pooledObjectSO.pooledObject}");
                    continue;
                }

                var objectPool = new ObjectPool<GameObject>(
                    createFunc: () => Object.Instantiate(pooledObjectSO.pooledObjectPrefab),
                    actionOnGet: obj =>
                    {
                        obj.gameObject.SetActive(true);
                        foreach (var poolable in obj.GetComponentsInChildren<IPoolable>())
                        {
                            if (poolable is PoolableParticleVFX particleVFX && !particleVFX.IsConfigured)
                            {
                                particleVFX.Configure(this, pooledObjectSO.pooledObject);
                            }
                            
                            poolable.OnSpawned();
                        }
                    },
                    actionOnRelease: obj =>
                    {
                        foreach (var poolable in obj.GetComponentsInChildren<IPoolable>())
                        {
                            poolable.OnReturned();
                        }
                        obj.gameObject.SetActive(false);
                    },
                    actionOnDestroy: Object.Destroy,
                    collectionCheck: false,
                    defaultCapacity: pooledObjectSO.minPoolSize,
                    maxSize: pooledObjectSO.maxPoolSize
                );
                
                _objectPools[pooledObjectSO.pooledObject] = objectPool;
            }
        }

        public GameObject Get(PooledObject pooledObject)
        {
            if (_objectPools.TryGetValue(pooledObject, out var objectPool))
            {
                return objectPool.Get();
            }
            
            Debug.LogError($"No object pool found for tag: {pooledObject}");
            return null;
        }

        public void Return(PooledObject pooledObject, GameObject pooledObjectPrefab)
        {
            if (_objectPools.TryGetValue(pooledObject, out var objectPool))
            {
                objectPool.Release(pooledObjectPrefab);
            }
            else
            {
                Debug.LogWarning($"Pool not found for tag: {pooledObject}");
                Object.Destroy(pooledObjectPrefab);
            }
        }
        
        public void Initialise()
        {
            
        }
    }
}