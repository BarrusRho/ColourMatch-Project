using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColourMatch
{
    public class DifficultyMenuView : ViewBase
    {
        [SerializeField] private Button easyButton;
        [SerializeField] private Button mediumButton;
        [SerializeField] private Button hardButton;
        
        public event Action<DifficultyLevel> OnDifficultyButtonClicked;
        
        protected override void OnShow()
        {
            Logger.BasicLog(this, "Difficulty menu shown.", LogChannel.UI);

            easyButton.onClick.AddListener(() => OnDifficultyButtonPressed(DifficultyLevel.Easy));
            mediumButton.onClick.AddListener(() => OnDifficultyButtonPressed(DifficultyLevel.Medium));
            hardButton.onClick.AddListener(() => OnDifficultyButtonPressed(DifficultyLevel.Hard));
        }

        protected override void OnHide()
        {
            Logger.BasicLog(this, "Difficulty menu hidden.", LogChannel.UI);

            easyButton.onClick.RemoveAllListeners();
            mediumButton.onClick.RemoveAllListeners();
            hardButton.onClick.RemoveAllListeners();
        }

        private void OnDifficultyButtonPressed(DifficultyLevel difficultyLevel)
        {
            Logger.BasicLog(this, $"Difficulty selected: {difficultyLevel}", LogChannel.UI);
            OnDifficultyButtonClicked?.Invoke(difficultyLevel);
        }
    }
}