using System;
using UnityEngine;

namespace ColourMatch
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameVariablesSO gameVariablesSO;
        
        [SerializeField] private SpriteRenderer playerSpriteRenderer;

        private string currentPlayerColour;
        
        private string previousPlayerColour;
        
        private int currentColourIndex = 0;

        public Action<Player> CollisionOccurredOnPlayer = delegate { };
        
        public void AssignPlayerRandomColour()
        {
            while (true)
            {
                var randomNumber = UnityEngine.Random.Range(0, 4);
                var newColorIndex = (currentColourIndex + randomNumber) % 4;

                if (newColorIndex == currentColourIndex)
                {
                    continue;
                }

                currentColourIndex = newColorIndex;
                UpdatePlayerColour();
                break;
            }
        }

        private void ChangePlayerColour(int direction)
        {
            currentColourIndex = (currentColourIndex + direction + 4) % 4;
            UpdatePlayerColour();
        }

        private void UpdatePlayerColour()
        {
            switch (currentColourIndex)
            {
                case 0:
                    currentPlayerColour = StringConstants.Magenta;
                    playerSpriteRenderer.color = gameVariablesSO.magentaColour;
                    break;

                case 1:
                    currentPlayerColour = StringConstants.Blue;
                    playerSpriteRenderer.color = gameVariablesSO.blueColour;
                    break;

                case 2:
                    currentPlayerColour = StringConstants.Green;
                    playerSpriteRenderer.color = gameVariablesSO.greenColour;
                    break;

                case 3:
                    currentPlayerColour = StringConstants.Red;
                    playerSpriteRenderer.color = gameVariablesSO.redColour;
                    break;
            }
        }

        public void IncrementPlayerColour()
        {
            ChangePlayerColour(1);
        }

        public void DecrementPlayerColour()
        {
            ChangePlayerColour(-1);
        }
        
        /// <summary>
        /// Triggered when the player collides with another object.
        /// </summary>
        /// <param name="other">The other object involved in the collision.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.TryGetComponent<Obstacle>(out var obstacle))
            {
                if (obstacle.CurrentObstacleColour == currentPlayerColour)
                {
                    AudioPlayer.ColourMatch();
                }
                else
                {
                    obstacle.ReturnObstacleToPool(obstacle);
                    CollisionOccurredOnPlayer(this);
                }
            }
        }
    }
}