using UnityEngine;
using UnityEngine.EventSystems;

namespace ColourMatch
{
    /// <summary>
    /// Button the player can press on screen to trigger an action in game.
    /// </summary>
    public class ControllerButton : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Is the button pressed?
        /// </summary>
        public bool IsButtonClicked { get; set; }

        /// <summary>
        ///  Inform the button that it is being pressed.
        /// </summary>
        /// <param name="eventData">Data about the press event</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            IsButtonClicked = true;
        }
    }
}