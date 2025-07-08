using System;
using UnityEngine;

namespace ColourMatch
{
    public class PlayerView : GameplayViewBase
    {
        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        [SerializeField] private Rigidbody2D playerRigidbody;
        
        public Rigidbody2D PlayerRigidbody => playerRigidbody;

        public event Action<ObstacleView> OnObstacleCollided;

        public void SetColour(Color colour)
        {
            playerSpriteRenderer.color = colour;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out ObstacleView obstacle))
            {
                OnObstacleCollided?.Invoke(obstacle);
            }
        }
    }
}