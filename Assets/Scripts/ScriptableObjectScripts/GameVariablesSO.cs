using UnityEngine;

namespace ColourMatch
{
    [CreateAssetMenu(fileName = "GameVariables", menuName = "ColourMatch/GameVariables")]
    public class GameVariablesSO : ScriptableObject
    {
        [Header("Colours")]
        public Color magentaColour;
        public Color blueColour;
        public Color greenColour;
        public Color redColour;
        
        [Header("Obstacle Speed")]
        public float easyDifficultySpeed = 3.0f;
        public float mediumDifficultySpeed = 5.0f;
        public float hardDifficultySpeed = 7.0f;

        [Header("Game Over")] public float gameOverDelay = 1.5f;
    }
}
