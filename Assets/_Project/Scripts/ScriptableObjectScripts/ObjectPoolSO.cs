using UnityEngine;

namespace ColourMatch
{
    [CreateAssetMenu(fileName = "ObjectPool", menuName = "ColourMatch/ObjectPool")]
    public class ObjectPoolSO : ScriptableObject
    {
        public PooledObject pooledObject;
        public GameObject pooledObjectPrefab;
        public int minPoolSize = 5;
        public int maxPoolSize = 20;
    }
}
