using System;
using UnityEngine;

namespace ColourMatch
{
    /// <summary>
    /// DifficultyMenu component, responsible for allowing the player to select a difficulty.
    /// </summary>
    public class DifficultyMenuUI : MonoBehaviour
    {
        private int selectedDifficultyIndex = 0;
        
        /// <summary>
        /// Delegate triggered when a difficulty is selected.
        /// </summary>
        public Action<StateManager.DifficultyLevels> DifficultySelected = delegate { };

        /// <summary>
        /// Triggered when the player selects a difficulty.
        /// </summary>
        public void OnDifficultySelectedButtonPressed()
        {
            var selectedDifficulty = (StateManager.DifficultyLevels)selectedDifficultyIndex;
            DifficultySelected(selectedDifficulty);
        }

        private void SetSelectedDifficulty(int difficultyIndex)
        {
            selectedDifficultyIndex = difficultyIndex;
        }

        public void OnEasyButtonPressed()
        {
            SetSelectedDifficulty(0);
        }

        public void OnMediumButtonPressed()
        {
            SetSelectedDifficulty(1);
        }

        public void OnHardButtonPressed()
        {
            SetSelectedDifficulty(2);
        }
    }
}