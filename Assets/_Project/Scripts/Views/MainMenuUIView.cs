using System;
using UnityEngine;
using UnityEngine.UI;

namespace ColourMatch
{
    public class MainMenuUIView : UIViewBase, IUIView
    {
        [SerializeField] private Button startGameButton;

        public event Action OnStartGameButtonClicked;
        
        protected override void OnShow()
        {
            Logger.BasicLog(this, "Main menu shown.", LogChannel.UI);
            startGameButton.onClick.AddListener(OnStartGameButtonPressed);
        }

        protected override void OnHide()
        {
            Logger.BasicLog(this, "Main menu hidden.", LogChannel.UI);
            startGameButton.onClick.RemoveListener(OnStartGameButtonPressed);
        }

        private void OnStartGameButtonPressed()
        {
            Logger.BasicLog(this, "Start game button pressed — firing event.", LogChannel.UI);
            OnStartGameButtonClicked?.Invoke();
        }
    }
}