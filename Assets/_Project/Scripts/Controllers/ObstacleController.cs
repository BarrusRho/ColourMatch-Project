using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ColourMatch
{
    public class ObstacleController : GameplayControllerBase<ObstacleView>, IGameplayController, IGameplaySystem, IFixedGameplayLoop, IResettable
    {
        private GameCamera gameCamera;
        private GameConfigService gameConfigService;
        private PoolingService poolingService;

        private readonly List<ObstacleView> activeObstacles = new();
        private DifficultyLevel currentDifficultyLevel;

        protected override void OnInit()
        {
            gameCamera = ServiceLocator.Get<GameCamera>();
            gameConfigService = ServiceLocator.Get<GameConfigService>();
            poolingService = ServiceLocator.Get<PoolingService>();
        }
        
        public void Initialise()
        {
            Logger.BasicLog(typeof(ObstacleController), "ObstacleController initialised", LogChannel.GameplayControllerService);
        }

        public void Reset()
        {
            foreach (var obstacle in activeObstacles)
            {
                obstacle.OnReturned();
                poolingService.Return(PooledObject.ObstacleView, obstacle.gameObject);
            }

            activeObstacles.Clear();
            SpawnObstacle();
        }

        public void Shutdown()
        {
            foreach (var obstacle in activeObstacles)
            {
                obstacle.OnReturned();
                poolingService.Return(PooledObject.ObstacleView, obstacle.gameObject);
            }

            activeObstacles.Clear();
        }

        public void FixedTick()
        {
            for (int i = activeObstacles.Count - 1; i >= 0; i--)
            {
                var obstacle = activeObstacles[i];
                obstacle.MoveDown(Time.fixedDeltaTime);

                var screenPosition = gameCamera.WorldPositionToScreenPosition(obstacle.transform.position);
                if (screenPosition.y < 0)
                {
                    obstacle.OnReturned();
                    poolingService.Return(PooledObject.ObstacleView, obstacle.gameObject);
                    activeObstacles.RemoveAt(i);
                    SpawnObstacle();
                }
            }
        }
        
        private void SpawnObstacle()
        {
            var pooledObject = poolingService.Get(PooledObject.ObstacleView);
            var obstacle = pooledObject.GetComponent<ObstacleView>();
            
            if (obstacle == null)
            {
                Logger.Error(typeof(ObstacleController), "Spawned object is missing ObstacleView component!", LogChannel.PoolingService);
                return;
            }

            var startPosition = gameCamera.ScreenPositionToWorldPosition(new Vector2(Screen.width * 0.5f, Screen.height));
            var speed = gameConfigService.GetSpeedByDifficulty(currentDifficultyLevel);
            
            var colourType = (ColourType)Random.Range(0, Enum.GetValues(typeof(ColourType)).Length);
            var colour = gameConfigService.GetColourByType(colourType);

            obstacle.SetColour(colour, colourType);
            obstacle.SetPosition(startPosition);
            obstacle.ObstacleSpeed = speed;
            
            obstacle.OnSpawned();

            activeObstacles.Add(obstacle);

            AudioPlayer.ObstacleSpawn();
        }
        
        public void SetDifficulty(DifficultyLevel difficultyLevel)
        {
            currentDifficultyLevel = difficultyLevel;
        }
    }
}
