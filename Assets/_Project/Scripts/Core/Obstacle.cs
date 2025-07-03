using System;
using UnityEngine;

namespace ColourMatch
{
    public class Obstacle : MonoBehaviour, IPoolable
    {
        private GameConfigService gameConfigService;
        
        [SerializeField] private SpriteRenderer obstacleSpriteRenderer;
        [SerializeField] private Rigidbody2D obstacleRigidBody;
        
        private static readonly ColourType[] availableColours = (ColourType[])Enum.GetValues(typeof(ColourType));
        private ColourType currentObstacleColour;
        public ColourType CurrentObstacleColour => currentObstacleColour;
        
        public float ObstacleSpeed = 1.0f;

        private void Awake()
        {
            gameConfigService = ServiceLocator.Get<GameConfigService>();
        } 

        private void FixedUpdate()
        {
            obstacleRigidBody.position += Vector2.down * (Time.fixedDeltaTime * ObstacleSpeed);
        }

        public void AssignObstacleRandomColour()
        {
            currentObstacleColour = (ColourType)UnityEngine.Random.Range(0, availableColours.Length);
            obstacleSpriteRenderer.color = gameConfigService.GetColourByType(currentObstacleColour);
            Logger.BasicLog(this, $"Assigned colour: {currentObstacleColour}", LogChannel.Gameplay);
        }
        
        public void SetPosition(Vector3 position)
        {
            obstacleRigidBody.position = position;
        }

        public void OnSpawned()
        {
            Logger.BasicLog(this, $"Obstacle has spawned", LogChannel.PoolingService);
        }

        public void OnReturned()
        {
            Logger.BasicLog(this, $"Obstacle has been returned", LogChannel.PoolingService);
        }
    }
}