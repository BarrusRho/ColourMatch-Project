using System;
using UnityEngine;

namespace ColourMatch
{
    /// <summary>
    /// Enemy component, responsible for providing the logic for the enemy.
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private GameVariablesSO gameVariablesSO;
        
        [SerializeField] private SpriteRenderer obstacleSpriteRenderer;

        /// <summary>
        /// Rigidbody2D component of the enemy.
        /// </summary>
        [SerializeField] private Rigidbody2D obstacleRigidBody = null;
        
        private string currentObstacleColour;
        
        private string previousObstacleColour;
        public string CurrentObstacleColour => currentObstacleColour;
        
        public float ObstacleSpeed = 1.0f;

        /// <summary>
        /// Delegate triggered when the obstacle collides with another object.
        /// </summary>
        public Action<Obstacle> ReturnObstacleToPool;

        /// <summary>
        /// Update the position of the enemy.
        /// </summary>
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

        /// <summary>
        /// Set the position of the enemy.
        /// </summary>
        /// <param name="position">A position in world space.</param>
        public void SetPosition(Vector3 position)
        {
            obstacleRigidBody.position = position;
        }

        public void InitialiseObstacleForPool(Action<Obstacle> returnObstacleToPool)
        {
            ReturnObstacleToPool = returnObstacleToPool;
        }
    }
}