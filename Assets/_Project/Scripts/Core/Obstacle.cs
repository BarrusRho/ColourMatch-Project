using System;
using UnityEngine;

namespace ColourMatch
{
    public class Obstacle : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameVariablesSO gameVariablesSO;
        [SerializeField] private SpriteRenderer obstacleSpriteRenderer;
        [SerializeField] private Rigidbody2D obstacleRigidBody = null;
        
        private string currentObstacleColour;
        private string previousObstacleColour; 
        public string CurrentObstacleColour => currentObstacleColour;
        
        public float ObstacleSpeed = 1.0f;
        
        private void FixedUpdate()
        {
            obstacleRigidBody.position += Vector2.down * (Time.fixedDeltaTime * ObstacleSpeed);
        }

        public void AssignObstacleRandomColour()
        {
            var randomNumber = UnityEngine.Random.Range(0, 4);
            switch (randomNumber)
            {
                case 0:
                    currentObstacleColour = StringConstants.Magenta;
                    obstacleSpriteRenderer.color = gameVariablesSO.magentaColour;
                    break;

                case 1:
                    currentObstacleColour = StringConstants.Blue;
                    obstacleSpriteRenderer.color = gameVariablesSO.blueColour;
                    break;

                case 2:
                    currentObstacleColour = StringConstants.Green;
                    obstacleSpriteRenderer.color = gameVariablesSO.greenColour;
                    break;

                case 3:
                    currentObstacleColour = StringConstants.Red;
                    obstacleSpriteRenderer.color = gameVariablesSO.redColour;
                    break;
            }
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