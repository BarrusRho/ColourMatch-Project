using System;
using UnityEngine;

namespace ColourMatch
{
    public class PlayerController : GameplayControllerBase<PlayerView>, IGameplayController, IGameplaySystem,
        IResettable
    {
        private GameConfigService gameConfigService;
        private GameCamera gameCamera;
        private PoolingService poolingService;
        private int colourIndex = 0;
        private bool isWrongMatch = false;
        private ColourType[] availableColours;
        private ColourType CurrentColourType { get; set; }

        protected override void OnInit()
        {
            gameConfigService = ServiceLocator.Get<GameConfigService>();
            gameCamera = ServiceLocator.Get<GameCamera>();
            poolingService = ServiceLocator.Get<PoolingService>();

            availableColours = (ColourType[])Enum.GetValues(typeof(ColourType));
            
            EventBus.Subscribe<LeftButtonClickedEvent>(OnLeftButtonClicked);
            EventBus.Subscribe<RightButtonClickedEvent>(OnRightButtonClicked);

            View.OnObstacleCollided -= OnColourMatchCollision;
            View.OnObstacleCollided += OnColourMatchCollision;

            ApplyCurrentColour();
        }
        
        public void Initialise()
        {
            Logger.BasicLog(typeof(PlayerController), "PlayerController initialised", LogChannel.Gameplay);
        }
        
        public void PrepareForGameStart()
        {
            Reset();
            SetPlayerVisibility(true);
            SetStartPosition();
        }

        private void SetPlayerVisibility(bool isVisible)
        {
            View.gameObject.SetActive(isVisible);
            View.PlayerRigidbody.gameObject.SetActive(isVisible);
        }

        private void SetStartPosition()
        {
            var fixedYPosition = Screen.height * 0.33f;
            var playerScreenPosition = new Vector2(Screen.width * 0.5f, fixedYPosition);
            var playerWorldPosition = gameCamera.ScreenPositionToWorldPosition(playerScreenPosition);
            View.PlayerRigidbody.position = playerWorldPosition;
        }

        private void AssignRandomColour()
        {
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, availableColours.Length);
            } while (newIndex == colourIndex);

            colourIndex = newIndex;
            ApplyCurrentColour();
        }

        public void IncrementColour()
        {
            colourIndex = (colourIndex + 1) % availableColours.Length;
            ApplyCurrentColour();
            AudioPlayer.ChangeColour();
        }

        public void DecrementColour()
        {
            colourIndex = (colourIndex - 1 + availableColours.Length) % availableColours.Length;
            ApplyCurrentColour();
            AudioPlayer.ChangeColour();
        }

        private void ApplyCurrentColour()
        {
            CurrentColourType = availableColours[colourIndex];
            View.SetColour(gameConfigService.GetColourByType(CurrentColourType));
            Logger.BasicLog(typeof(PlayerController), $"Applied colour: {CurrentColourType}", LogChannel.Gameplay);
        }

        public void Reset()
        {
            isWrongMatch = false;
            AssignRandomColour();
            Logger.BasicLog(typeof(PlayerController), "Player reset and new colour assigned.", LogChannel.Gameplay);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<LeftButtonClickedEvent>(OnLeftButtonClicked);
            EventBus.Unsubscribe<RightButtonClickedEvent>(OnRightButtonClicked);
            Logger.BasicLog(typeof(PlayerController), "PlayerController shut down", LogChannel.Gameplay);
        }
        
        private void OnLeftButtonClicked(LeftButtonClickedEvent leftButtonClickedEvent)
        {
            DecrementColour();
            Logger.BasicLog(this, "Left button input received — changing player colour.", LogChannel.Gameplay);
        }
        
        private void OnRightButtonClicked(RightButtonClickedEvent rightButtonClickedEvent)
        {
            IncrementColour();
            Logger.BasicLog(this, "Right button input received — changing player colour.", LogChannel.Gameplay);
        }
        
        public void HandlePlayerCollision()
        {
            var playerImpactVFX = poolingService.Get(PooledObject.PlayerImpactVFX);
            playerImpactVFX.transform.position = View.PlayerRigidbody.position;
            playerImpactVFX.transform.rotation = Quaternion.identity;
            
            AudioPlayer.PlayerImpact();
            SetPlayerVisibility(false);
        }

        private void OnColourMatchCollision(ObstacleView obstacle)
        {
            if (isWrongMatch)
            {
                return;
            }

            if (obstacle.CurrentColour == CurrentColourType)
            {
                AudioPlayer.ColourMatch();
            }
            else
            {
                Logger.Warning(this,
                    $"Colour mismatch! Obstacle: {obstacle.CurrentColour}, Player: {CurrentColourType}",
                    LogChannel.Gameplay);
                poolingService.Return(PooledObject.ObstacleView, obstacle.gameObject);
                isWrongMatch = true;
                EventBus.Fire(new ColourMismatchEvent());
            }
        }
    }
}