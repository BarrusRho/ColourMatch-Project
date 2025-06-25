using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        private AudioService audioService;
        private PoolingService poolingService;
        
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
            audioService = new AudioService();
            poolingService = new PoolingService(objectPools);
        }

        private void RegisterServices()
        {
            ServiceLocator.RegisterOnce(gameCamera);
            ServiceLocator.RegisterOnce(audioService);
            ServiceLocator.RegisterOnce(poolingService);
            ServiceLocator.RegisterOnce(gameManager);
            ServiceLocator.RegisterOnce(stateManager);
        }

        private void InitialiseServices()
        {
            gameCamera.Initialise();
            audioService.Initialise(audioClips, audioSources);
            poolingService.Initialise();
            gameManager.Initialise();
            stateManager.Initialise();
        }
    }
}
