using System;
using UnityEngine;

namespace ColourMatch
{
    /// <summary>
    /// MainMenu component, responsible for beginning a game.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        /// <summary>
        /// Delegate triggered when the Start Game button is pressed.
        /// </summary>
        public Action StartGame = delegate { };

        /// <summary>
        /// Begins the process of starting the game.
        /// </summary>
        public void StartGameButtonPressed()
        {
            StartGame();
        }
    }
}