namespace ColourMatch
{
    public class PlayerController : ControllerBase<PlayerView>
    {
        private GameConfigService gameConfigService;
        private PoolingService poolingService;

        private int colourIndex = 0;
        private bool isWrongMatch = false;

        private readonly string[] colours =
        {
            StringConstants.Magenta,
            StringConstants.Blue,
            StringConstants.Green,
            StringConstants.Red
        };
        
        public string CurrentColourName {get; private set;}
        
        protected override void OnInit()
        {
            gameConfigService = ServiceLocator.Get<GameConfigService>();
            poolingService = ServiceLocator.Get<PoolingService>();

            View.OnObstacleCollided += OnColourMatchCollision;
        }
        
        public void AssignRandomColour()
        {
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, colours.Length);
            } while (newIndex == colourIndex);

            colourIndex = newIndex;
            ApplyColour();
        }
        
        public void IncrementColour()
        {
            colourIndex = (colourIndex + 1) % colours.Length;
            ApplyColour();
            AudioPlayer.ChangeColour();
        }
        
        public void DecrementColour()
        {
            colourIndex = (colourIndex - 1 + colours.Length) % colours.Length;
            ApplyColour();
            AudioPlayer.ChangeColour();
        }

        private void ApplyColour()
        {
            CurrentColourName = colours[colourIndex];
            View.SetColour(gameConfigService.GetColourByName(CurrentColourName));
        }

        public void Reset()
        {
            isWrongMatch = false;
            colourIndex = 0;
            AssignRandomColour();
        }

        private void OnColourMatchCollision(Obstacle obstacle)
        {
            if (isWrongMatch)
            {
                return;
            }
            
            if (obstacle.CurrentObstacleColour == CurrentColourName)
            {
                AudioPlayer.ColourMatch();
            }
            else
            {
                Logger.Warning(this, $"Colour mismatch! Obstacle: {obstacle.CurrentObstacleColour}, Player: {CurrentColourName}", LogChannel.Gameplay);
                poolingService.Return(PooledObject.Obstacle, obstacle.gameObject);
                isWrongMatch = true;
                EventBus.Fire(new ColourMismatchEvent());
            }
        }
    }
}
