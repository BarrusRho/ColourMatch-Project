using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        private AudioManager audioManager;
        private PoolManager poolManager;
        
        [SerializeField] private GameCamera gameCamera;

        [SerializeField] private StateManager stateManager;
        [SerializeField] private GameManager gameManager;
        
        [SerializeField] private AudioClipsSO audioClips;
        [SerializeField] private AudioSource[] audioSources;
        
        [SerializeField] private List<ObjectPoolSO> objectPools;
        
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
            poolManager = new PoolManager(objectPools);
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
