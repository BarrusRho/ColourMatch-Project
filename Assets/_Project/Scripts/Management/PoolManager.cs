using UnityEngine;
using UnityEngine.Pool;

namespace ColourMatch
{
    public class PoolManager : MonoBehaviourServiceUser
    {
        [SerializeField] private Obstacle obstaclePrefab;
        [SerializeField] private int minPoolSize = 10;
        [SerializeField] private int maxPoolSize = 20;

        private ObjectPool<Obstacle> obstaclePool;
        
        public void Initialise()
        {
            CreateObstaclePool();
        }

        private void CreateObstaclePool()
        {
            obstaclePool = new ObjectPool<Obstacle>(() => { return Instantiate(obstaclePrefab); },
                obstacle => { obstacle.gameObject.SetActive(true); },
                obstacle => { obstacle.gameObject.SetActive(false); },
                obstacle => { Destroy(obstacle.gameObject); },
                true, minPoolSize, maxPoolSize);
        }

        public Obstacle GetObstacleFromPool()
        {
            var obstacle = obstaclePool.Get();
            obstacle.InitialiseObstacleForPool(ReturnObstacleToPool);
            return obstacle;
        }

        private void ReturnObstacleToPool(Obstacle obstacle)
        {
            obstaclePool.Release(obstacle);
        }

    }
}