using System;

namespace ColourMatch
{
    public class PlayerController : ControllerBase<PlayerView>
    {
        private GameConfigService gameConfigService;
        private PoolingService poolingService;

        private int colourIndex = 0;
        private bool isWrongMatch = false;
        
        private ColourType[] availableColours;
        private ColourType CurrentColourType {get; set;}
        
        protected override void OnInit()
        {
            gameConfigService = ServiceLocator.Get<GameConfigService>();
            poolingService = ServiceLocator.Get<PoolingService>();
            
            availableColours = (ColourType[])Enum.GetValues(typeof(ColourType));

            View.OnObstacleCollided += OnColourMatchCollision;
        }

        private void AssignRandomColour()
        {
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, availableColours.Length);
            } while (newIndex == colourIndex);

            colourIndex = newIndex;
            ApplyColour();
        }
        
        public void IncrementColour()
        {
            colourIndex = (colourIndex + 1) % availableColours.Length;
            ApplyColour();
            AudioPlayer.ChangeColour();
        }
        
        public void DecrementColour()
        {
            colourIndex = (colourIndex - 1 + availableColours.Length) % availableColours.Length;
            ApplyColour();
            AudioPlayer.ChangeColour();
        }

        private void ApplyColour()
        {
            CurrentColourType = availableColours[colourIndex];
            View.SetColour(gameConfigService.GetColourByType(CurrentColourType));
            Logger.BasicLog(typeof(PlayerController), $"Applied colour: {CurrentColourType}", LogChannel.Gameplay);
        }

        public void Reset()
        {
            isWrongMatch = false;
            AssignRandomColour();
        }

        private void OnColourMatchCollision(Obstacle obstacle)
        {
            if (isWrongMatch)
            {
                return;
            }
            
            if (obstacle.CurrentObstacleColour == CurrentColourType)
            {
                AudioPlayer.ColourMatch();
            }
            else
            {
                Logger.Warning(this, $"Colour mismatch! Obstacle: {obstacle.CurrentObstacleColour}, Player: {CurrentColourType}", LogChannel.Gameplay);
                poolingService.Return(PooledObject.Obstacle, obstacle.gameObject);
                isWrongMatch = true;
                EventBus.Fire(new ColourMismatchEvent());
            }
        }
    }
}
