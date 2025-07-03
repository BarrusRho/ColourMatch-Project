using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class GameConfigService
    {
        private readonly Dictionary<ColourType, Color> colourMap;

        public float EasySpeed { get; }
        public float MediumSpeed { get; }
        public float HardSpeed { get; }
        
        public float GameOverDelay { get; }

        public GameConfigService(GameVariablesSO gameVariablesSO)
        {
            colourMap = new Dictionary<ColourType, Color>()
            {
                { ColourType.Magenta, gameVariablesSO.magentaColour },
                { ColourType.Blue, gameVariablesSO.blueColour },
                { ColourType.Green, gameVariablesSO.greenColour },
                { ColourType.Red, gameVariablesSO.redColour }
            };

            EasySpeed = gameVariablesSO.easyDifficultySpeed;
            MediumSpeed = gameVariablesSO.mediumDifficultySpeed;
            HardSpeed = gameVariablesSO.hardDifficultySpeed;
            
            GameOverDelay = gameVariablesSO.gameOverDelay;
        }

        public void Initialise()
        {
            
        }
        
        public Color GetColourByType(ColourType colourType)
        {
            return colourMap.TryGetValue(colourType, out var color) ? color : Color.white;
        }
        
        public float GetSpeedByDifficulty(DifficultyLevel difficulty)
        {
            switch (difficulty)
            {
                case DifficultyLevel.Easy:
                    return EasySpeed;
                case DifficultyLevel.Medium:
                    return MediumSpeed;
                case DifficultyLevel.Hard:
                    return HardSpeed;
                default:
                    return 1f;
            }
        }
    }
}
