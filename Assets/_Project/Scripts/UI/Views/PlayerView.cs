using System;
using UnityEngine;

namespace ColourMatch
{
    public class PlayerView : ViewBase
    {
        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        [SerializeField] private Rigidbody2D playerRigidbody;
        
        public Rigidbody2D PlayerRigidbody => playerRigidbody;

        public event Action<Obstacle> OnObstacleCollided;

        public void SetColour(Color colour)
        {
            playerSpriteRenderer.color = colour;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
            {
                OnObstacleCollided?.Invoke(obstacle);
            }
        }
    }
}