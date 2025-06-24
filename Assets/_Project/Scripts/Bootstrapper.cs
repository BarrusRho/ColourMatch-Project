using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        private AudioManager audioManager;
        
        [SerializeField] private GameCamera gameCamera;

        [SerializeField] private StateManager stateManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PoolManager poolManager;
        
        [SerializeField] private AudioClipsSO audioClips;
        [SerializeField] private AudioSource[] audioSources;
        
        private void Awake()
        { 
            CreateServices();
            RegisterServices();
            InitialiseServices();
            Debug.Log("Bootstrapper: All components initialized and registered.");
        }

        private void CreateServices()
        {
            audioManager = new AudioManager();
        }

        private void RegisterServices()
        {
            ServiceLocator.RegisterOnce(gameCamera);
            ServiceLocator.RegisterOnce(audioManager);
            ServiceLocator.RegisterOnce(poolManager);
            ServiceLocator.RegisterOnce(gameManager);
            ServiceLocator.RegisterOnce(stateManager);
        }

        private void InitialiseServices()
        {
            gameCamera.Initialise();
            audioManager.Initialise(audioClips, audioSources);
            poolManager.Initialise();
            gameManager.Initialise();
            stateManager.Initialise();
        }
    }
}
