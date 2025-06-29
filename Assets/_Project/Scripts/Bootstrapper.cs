using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        private AudioService audioService;
        private PoolingService poolingService;
        private ControllerService controllerService;
        private GameStateService gameStateService;
        
        [SerializeField] private GameCamera gameCamera;

        [SerializeField] private GameManager gameManager;
        
        [SerializeField] private AudioClipsSO audioClips;
        [SerializeField] private AudioSource[] audioSources;
        
        [SerializeField] private List<ObjectPoolSO> objectPools;
        
        [SerializeField] private ViewRegistry viewRegistry;
        
        [SerializeField] private LogChannelsSO logChannels;
        
        private void Awake()
        { 
            Logger.SetChannels(logChannels);
            Logger.FirstTouch();
            
            CreateServices();
            RegisterServices();
            InitialiseServices();
            Logger.BasicLog(this, $"Bootstrapper: All components initialized and registered.", LogChannel.BasicLog);
        }

        private void CreateServices()
        {
            controllerService = new ControllerService(viewRegistry);
            audioService = new AudioService();
            gameStateService = new GameStateService(controllerService);
            poolingService = new PoolingService(objectPools);
        }

        private void RegisterServices()
        {
            ServiceLocator.RegisterOnce(gameCamera);
            ServiceLocator.RegisterOnce(controllerService);
            ServiceLocator.RegisterOnce(audioService);
            ServiceLocator.RegisterOnce(gameStateService);
            ServiceLocator.RegisterOnce(poolingService);
            ServiceLocator.RegisterOnce(gameManager);
        }

        private void InitialiseServices()
        {
            gameCamera.Initialise();
            controllerService.Initialise();
            audioService.Initialise(audioClips, audioSources);
            gameStateService.Initialise();
            poolingService.Initialise();
            gameManager.Initialise();
        }

        private void OnDestroy()
        {
            ServiceLocator.Get<GameStateService>().Dispose();
        }
    }
}
