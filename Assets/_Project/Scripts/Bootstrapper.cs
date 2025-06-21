using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private GameCamera gameCamera;
        
        private void Awake()
        {
                InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            gameCamera.Initialise();
        }
    }
}
