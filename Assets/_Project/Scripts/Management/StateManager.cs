using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ColourMatch
{
    public class StateManager : MonoBehaviour
    {
        /// <summary>
        /// States of the game.
        /// </summary>
        private enum GameStates
        {
            /// <summary>
            /// Initial setup.
            /// </summary>
            Init,

            /// <summary>
            /// Player is in the main menu.
            /// </summary>
            MainMenu,

            /// <summary>
            /// Player is in the difficulty menu.
            /// </summary>
            DifficultyMenu,

            /// <summary>
            /// Player is playing the game.
            /// </summary>
            Game
        }

        public enum DifficultyLevels
        {
            Easy,
            Medium,
            Hard
        }

        /// <summary>
        /// Game component on the toys GameObject.
        /// </summary>
        [SerializeField] private GameManager gameManager = null;

        /// <summary>
        /// MainMenu component on the main menu GameObject.
        /// </summary>
        [SerializeField] private MainMenuUI mainMenuUI = null;

        /// <summary>
        /// DifficultyMenu component on the difficulty GameObject.
        /// </summary>
        [SerializeField] private DifficultyMenuUI difficultyMenuUI = null;

        /// <summary>
        /// The current state of the game.
        /// </summary>
        private GameStates state = GameStates.Init;

        public DifficultyLevels selectedDifficulty;

        public Task InitialiseAsync()
        {
            Initialise();
            return Task.CompletedTask;
        }

        private void Initialise()
        {
            State = GameStates.MainMenu;
            AudioPlayer.NewGameStart();
        }
        
        private void OnEnable()
        {
            mainMenuUI.StartGame += OnStartGame;
            difficultyMenuUI.DifficultySelected += OnDifficultySelected;
            gameManager.GameComplete += OnGameComplete;
        }

        private void OnDisable()
        {
            mainMenuUI.StartGame -= OnStartGame;
            difficultyMenuUI.DifficultySelected -= OnDifficultySelected;
            gameManager.GameComplete -= OnGameComplete;
        }

        /// <summary>
        /// State of the game. Changing the state will transition the game.
        /// </summary>
        private GameStates State
        {
            get => state;

            set
            {
                // Cannot return to init.
                if (value == GameStates.Init)
                {
                    throw new ArgumentException("Cannot return to init state.");
                }

                state = value;

                switch (state)
                {
                    case GameStates.MainMenu:
                        mainMenuUI.gameObject.SetActive(true);
                        difficultyMenuUI.gameObject.SetActive(false);
                        gameManager.gameObject.SetActive(false);
                        break;

                    case GameStates.DifficultyMenu:
                        mainMenuUI.gameObject.SetActive(false);
                        difficultyMenuUI.gameObject.SetActive(true);
                        gameManager.gameObject.SetActive(false);
                        break;

                    case GameStates.Game:
                        mainMenuUI.gameObject.SetActive(false);
                        difficultyMenuUI.gameObject.SetActive(false);
                        gameManager.gameObject.SetActive(true);
                        break;

                    case GameStates.Init:
                    // Intentional fallthrough. Init isn't supported but will be explicitly handled above.
                    default:
                        throw new ArgumentOutOfRangeException($"State machine doesn't support state {state}.");
                }
            }
        }

        /// <summary>
        /// Triggered when the player presses the start game button.
        /// </summary>
        private void OnStartGame()
        {
            State = GameStates.DifficultyMenu;
            AudioPlayer.Click();
            AudioPlayer.Confirm();
        }

        /// <summary>
        /// Triggered when the player chooses a difficulty.
        /// </summary>
        private void OnDifficultySelected(DifficultyLevels difficulty)
        {
            selectedDifficulty = difficulty;
            gameManager.SetStateManager(this);
            State = GameStates.Game;
            gameManager.InitialiseGame();
            AudioPlayer.Click();
            AudioPlayer.Confirm();
        }

        /// <summary>
        /// Triggered when the player completes a game.
        /// </summary>
        private void OnGameComplete()
        {
            State = GameStates.MainMenu;
            AudioPlayer.NewGameStart();
        }
    }
}