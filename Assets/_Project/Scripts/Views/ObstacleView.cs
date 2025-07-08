using UnityEngine;

namespace ColourMatch
{
    public class ObstacleView : GameplayViewBase, IPoolable
    {
        [SerializeField] private SpriteRenderer obstacleSpriteRenderer;
        [SerializeField] private Rigidbody2D obstacleRigidBody;
        
        public Rigidbody2D Rigidbody => obstacleRigidBody;
        public float ObstacleSpeed { get; set; }
        public ColourType CurrentColour { get; private set; }
        
        public void SetColour(Color colour, ColourType colourType)
        {
            obstacleSpriteRenderer.color = colour;
            CurrentColour = colourType;
        }
        
        public void SetPosition(Vector3 position)
        {
            obstacleRigidBody.position = position;
        }
        
        public void MoveDown(float deltaTime)
        {
            obstacleRigidBody.position += Vector2.down * (deltaTime * ObstacleSpeed);
        }
        
        public void OnSpawned()
        {
            Logger.BasicLog(this, "Obstacle spawned", LogChannel.PoolingService);
        }

        public void OnReturned()
        {
            Logger.BasicLog(this, "Obstacle returned to pool", LogChannel.PoolingService);
        }
    }
}
