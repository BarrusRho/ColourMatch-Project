using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ColourMatch
{
    public class PoolingService
    {
        private readonly Dictionary<PooledObject, IObjectPool<GameObject>> _objectPools = new();

        public PoolingService(IEnumerable<ObjectPoolSO> pooledObjects)
        {
            foreach (var pooledObjectSO in pooledObjects)
            {
                if (pooledObjectSO.pooledObjectPrefab == null)
                {
                    Logger.Warning(typeof(PoolingService), $"Skipping pool config with missing prefab for tag: {pooledObjectSO.pooledObject}", LogChannel.PoolingService);
                    continue;
                }

                if (_objectPools.ContainsKey(pooledObjectSO.pooledObject))
                {
                    Logger.Warning(typeof(PoolingService), $"Duplicate pool tag: {pooledObjectSO.pooledObject}", LogChannel.PoolingService);
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
            
            Logger.Error(typeof(PoolingService), $"No object pool found for tag: {pooledObject}", LogChannel.PoolingService);
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
                Logger.Error(typeof(PoolingService), $"Pool not found for tag: {pooledObject}", LogChannel.PoolingService);
                Object.Destroy(pooledObjectPrefab);
            }
        }
        
        public void Initialise()
        {
            
        }
    }
}