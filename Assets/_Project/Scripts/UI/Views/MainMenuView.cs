using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColourMatch
{
    public class MainMenuView : ViewBase
    {
        [SerializeField] private Button startGameButton;

        public event Action OnStartButtonClicked;
        
        protected override void OnShow()
        {
            Logger.BasicLog(this, "Main menu shown.", LogChannel.UI);
            startGameButton.onClick.AddListener(StartGameButtonPressed);
        }

        protected override void OnHide()
        {
            Logger.BasicLog(this, "Main menu hidden.", LogChannel.UI);
            startGameButton.onClick.RemoveListener(StartGameButtonPressed);
        }

        private void StartGameButtonPressed()
        {
            Logger.BasicLog(this, "Start game button pressed — firing event.", LogChannel.UI);
            OnStartButtonClicked?.Invoke();
        }
    }
}