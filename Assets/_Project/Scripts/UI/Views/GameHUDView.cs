using System;
using UnityEngine;

namespace ColourMatch
{
    public class GameHUDView : ViewBase
    {
        [SerializeField] private ClickButtonView leftButton;
        [SerializeField] private ClickButtonView rightButton;
        
        public ClickButtonView LeftButton => leftButton;
        public ClickButtonView RightButton => rightButton;
        
        public event Action OnLeftButtonClicked;
        public event Action OnRightButtonClicked;
        
        protected override void OnShow()
        {
            Logger.BasicLog(this, "GameHUD shown.", LogChannel.UI);

            leftButton.OnClicked += HandleLeftClicked;
            rightButton.OnClicked += HandleRightClicked;
        }

        protected override void OnHide()
        {
            Logger.BasicLog(this, "GameHUD hidden.", LogChannel.UI);

            leftButton.OnClicked -= HandleLeftClicked;
            rightButton.OnClicked -= HandleRightClicked;
        }

        private void HandleLeftClicked()
        {
            Logger.BasicLog(this, "Left button clicked.", LogChannel.UI);
            OnLeftButtonClicked?.Invoke();
        }

        private void HandleRightClicked()
        {
            Logger.BasicLog(this, "Right button clicked.", LogChannel.UI);
            OnRightButtonClicked?.Invoke();
        }
    }
}