using UnityEngine;

namespace ColourMatch
{
    /// <summary>
    /// GameHUD component, responsible for providing game UI.
    /// </summary>
    public class GameHUDView : ViewBase
    {
        /// <summary>
        /// UnityProperty field for the left ControllerButton.
        /// </summary>
        [SerializeField] private ControllerButton leftButton;

        /// <summary>
        /// UnityProperty field for the right ControllerButton.
        /// </summary>
        [SerializeField] private ControllerButton rightButton;

        /// <summary>
        /// ControllerButton for moving the player left.
        /// </summary>
        public ControllerButton LeftButton => leftButton;

        /// <summary>
        /// ControllerButton for moving the player right.
        /// </summary>
        public ControllerButton RightButton => rightButton;
        
        protected override void OnShow()
        {
            Logger.BasicLog(this, "GameHUD shown.", LogChannel.UI);
        }

        protected override void OnHide()
        {
            Logger.BasicLog(this, "GameHUD hidden.", LogChannel.UI);
        }
    }
}