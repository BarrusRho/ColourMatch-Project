using UnityEngine;

namespace ColourMatch
{
    /// <summary>
    /// GameHUD component, responsible for providing game UI.
    /// </summary>
    public class GameHUD : MonoBehaviour
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
    }
}