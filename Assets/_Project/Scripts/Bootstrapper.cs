using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameConfigService gameConfigService;
        private AudioService audioService;
        private PoolingService poolingService;
        private UIControllerService uiControllerService;
        private GameplayControllerService gameplayControllerService;
        private GameStateService gameStateService;
        private GameplaySystemService gameplaySystemService;
        
        [SerializeField] private GameVariablesSO gameVariablesSO;
        
        [SerializeField] private GameCamera gameCamera;

        [SerializeField] private GameplayCoordinator gameplayCoordinator;
        
        [SerializeField] private AudioClipsSO audioClips;
        [SerializeField] private AudioSource[] audioSources;
        
        [SerializeField] private List<ObjectPoolSO> objectPools;
        
        [SerializeField] private UIViewRegistry uiViewRegistry;
        
        [SerializeField] private LogChannelsSO logChannels;
        
        private void Awake()
        { 
            Logger.SetChannels(logChannels);
            Logger.FirstTouch();
            
            CreateServices();
            RegisterServices();
            InitialiseServices();
            InitialiseCoordinators();
            Logger.BasicLog(this, $"Bootstrapper: All components initialized and registered.", LogChannel.BasicLog);
        }

        private void CreateServices()
        {
            gameConfigService = new GameConfigService(gameVariablesSO);
            uiControllerService = new UIControllerService(uiViewRegistry);
            gameplayControllerService = new GameplayControllerService();
            audioService = new AudioService();
            gameStateService = new GameStateService();
            poolingService = new PoolingService(objectPools);
            
            gameplaySystemService = gameObject.AddComponent<GameplaySystemService>();
        }

        private void RegisterServices()
        {
            ServiceLocator.RegisterOnce(gameConfigService);
            ServiceLocator.RegisterOnce(gameCamera);
            ServiceLocator.RegisterOnce(uiControllerService);
            ServiceLocator.RegisterOnce(gameplayControllerService);
            ServiceLocator.RegisterOnce(audioService);
            ServiceLocator.RegisterOnce(gameStateService);
            ServiceLocator.RegisterOnce(poolingService);
            ServiceLocator.RegisterOnce(gameplaySystemService);
        }

        private void InitialiseServices()
        {
            gameConfigService.Initialise();
            gameCamera.Initialise();
            uiControllerService.Initialise();
            gameplayControllerService.Initialise();
            audioService.Initialise(audioClips, audioSources);
            gameStateService.Initialise();
            poolingService.Initialise();
            gameplaySystemService.Initialise();
        }

        private void InitialiseCoordinators()
        {
            gameplayCoordinator.Initialise();
        }

        private void OnDestroy()
        {
            gameStateService?.Dispose();
            uiControllerService?.Dispose();
            gameplayControllerService?.Dispose();
            gameplaySystemService?.Dispose();
            
            ServiceLocator.Unregister<GameStateService>();
            ServiceLocator.Unregister<UIControllerService>();
            ServiceLocator.Unregister<GameplayControllerService>();
            ServiceLocator.Unregister<GameConfigService>();
            ServiceLocator.Unregister<GameCamera>();
            ServiceLocator.Unregister<AudioService>();
            ServiceLocator.Unregister<PoolingService>();
            ServiceLocator.Unregister<GameplaySystemService>();
        }
    }
}
