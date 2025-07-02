using UnityEngine;

namespace ColourMatch
{
    public class GameConfigService
    {
        public Color MagentaColour { get; }
        public Color BlueColour { get; }
        public Color GreenColour { get; }
        public Color RedColour { get; }

        public float EasySpeed { get; }
        public float MediumSpeed { get; }
        public float HardSpeed { get; }
        
        public float GameOverDelay { get; }

        public GameConfigService(GameVariablesSO gameVariablesSO)
        {
            MagentaColour = gameVariablesSO.magentaColour;
            BlueColour = gameVariablesSO.blueColour;
            GreenColour = gameVariablesSO.greenColour;
            RedColour = gameVariablesSO.redColour;

            EasySpeed = gameVariablesSO.easyDifficultySpeed;
            MediumSpeed = gameVariablesSO.mediumDifficultySpeed;
            HardSpeed = gameVariablesSO.hardDifficultySpeed;
            
            GameOverDelay = gameVariablesSO.gameOverDelay;
        }

        public void Initialise()
        {
            
        }
        
        public Color GetColourByName(string name)
        {
            switch (name)
            {
                case StringConstants.Magenta:
                    return MagentaColour;
                case StringConstants.Blue:
                    return BlueColour;
                case StringConstants.Green:
                    return GreenColour;
                case StringConstants.Red:
                    return RedColour;
                default:
                    return Color.white;
            }
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
