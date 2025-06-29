using System;
using UnityEngine;

namespace ColourMatch
{
    public class StateManager : MonoBehaviourServiceUser
    {
        private GameManager gameManager;
        private ControllerService controllerService;
        
        private GameState state = GameState.Init;
        public DifficultyLevel selectedDifficulty;

        public void Initialise()
        {
            gameManager = ResolveServiceDependency<GameManager>();
            controllerService = ResolveServiceDependency<ControllerService>();

            State = GameState.MainMenu;
            AudioPlayer.NewGameStart();

            EventBus.Subscribe<GameStartEvent>(OnStartGame);
            EventBus.Subscribe<DifficultySelectedEvent>(OnDifficultySelected);
            EventBus.Subscribe<GameCompleteEvent>(OnGameComplete);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameStartEvent>(OnStartGame);
            EventBus.Unsubscribe<DifficultySelectedEvent>(OnDifficultySelected);
            EventBus.Unsubscribe<GameCompleteEvent>(OnGameComplete);
        }

        /// <summary>
        /// State of the game. Changing the state will transition the game.
        /// </summary>
        private GameState State
        {
            get => state;

            set
            {
                // Cannot return to init.
                if (value == GameState.Init)
                {
                    Logger.Error(typeof(StateManager), "Attempted illegal transition to Init state.",
                        LogChannel.Services);
                    throw new ArgumentException("Cannot return to init state.");
                }

                state = value;
                
                controllerService.HideAll();
                gameManager.gameObject.SetActive(false);

                switch (state)
                {
                    case GameState.MainMenu:
                        controllerService.Show(ViewType.MainMenu);
                        break;

                    case GameState.DifficultyMenu:
                        controllerService.Show(ViewType.DifficultyMenu);
                        break;

                    case GameState.Game:
                        controllerService.Show(ViewType.GameHUD);
                        gameManager.gameObject.SetActive(true);
                        break;

                    case GameState.Init:
                    // Intentional fallthrough. Init isn't supported but will be explicitly handled above.
                    default:
                        Logger.Error(typeof(StateManager), $"Invalid state transition attempted: {state}",
                            LogChannel.Services);
                        throw new ArgumentOutOfRangeException($"State machine doesn't support state {state}.");
                }
            }
        }

        /// <summary>
        /// Triggered when the player presses the start game button.
        /// </summary>
        private void OnStartGame(GameStartEvent gameStartEvent)
        {
            Logger.BasicLog(typeof(StateManager), "StartGameEvent received — changing state.", LogChannel.UI);
            State = GameState.DifficultyMenu;
            AudioPlayer.Click();
            AudioPlayer.Confirm();
        }

        /// <summary>
        /// Triggered when the player chooses a difficulty.
        /// </summary>
        private void OnDifficultySelected(DifficultySelectedEvent difficultySelectedEvent)
        {
            selectedDifficulty = difficultySelectedEvent.DifficultyLevel;
            State = GameState.Game;
            gameManager.InitialiseGame();
            AudioPlayer.Click();
            AudioPlayer.Confirm();
        }

        /// <summary>
        /// Triggered when the player completes a game.
        /// </summary>
        private void OnGameComplete(GameCompleteEvent gameCompleteEvent)
        {
            State = GameState.MainMenu;
            AudioPlayer.NewGameStart();
        }
    }
}