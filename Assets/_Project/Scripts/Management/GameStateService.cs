using System;

namespace ColourMatch
{
    public class GameStateService
    {
        private GameState state = GameState.Init;
        private DifficultyLevel selectedDifficulty;

        public GameStateService()
        {
        }

        public void Initialise()
        {
            State = GameState.MainMenu;
            AudioPlayer.NewGameStart();

            EventBus.Subscribe<GameStartEvent>(OnStartGame);
            EventBus.Subscribe<DifficultySelectedEvent>(OnDifficultySelected);
            EventBus.Subscribe<GameCompleteEvent>(OnGameComplete);
        }

        public void Dispose()
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
                if (value == GameState.Init)
                {
                    Logger.Error(typeof(GameStateService), "Attempted illegal transition to Init state.",
                        LogChannel.Services);
                    throw new ArgumentException("Cannot return to init state.");
                }

                state = value;

                switch (state)
                {
                    case GameState.MainMenu:
                        EventBus.Fire(new ViewTransitionEvent(ViewType.MainMenu));
                        break;

                    case GameState.DifficultyMenu:
                        EventBus.Fire(new ViewTransitionEvent(ViewType.DifficultyMenu));
                        break;

                    case GameState.Game:
                        EventBus.Fire(new ViewTransitionEvent(ViewType.GameHUD));
                        break;

                    case GameState.Init:
                    default:
                        Logger.Error(typeof(GameStateService), $"Invalid state transition attempted: {state}",
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
            Logger.BasicLog(typeof(GameStateService), "StartGameEvent received — changing state.", LogChannel.UI);
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
            
            EventBus.Fire(new GameBeginEvent(selectedDifficulty));
            
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