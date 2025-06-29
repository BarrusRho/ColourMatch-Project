using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    public class Bootstrapper : MonoBehaviour
    {
        private AudioService audioService;
        private PoolingService poolingService;
        private ControllerService controllerService;
        
        [SerializeField] private GameCamera gameCamera;

        [SerializeField] private StateManager stateManager;
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
            poolingService = new PoolingService(objectPools);
        }

        private void RegisterServices()
        {
            ServiceLocator.RegisterOnce(gameCamera);
            ServiceLocator.Register(controllerService);
            ServiceLocator.RegisterOnce(audioService);
            ServiceLocator.RegisterOnce(poolingService);
            ServiceLocator.RegisterOnce(gameManager);
            ServiceLocator.RegisterOnce(stateManager);
        }

        private void InitialiseServices()
        {
            gameCamera.Initialise();
            controllerService.Initialise();
            audioService.Initialise(audioClips, audioSources);
            poolingService.Initialise();
            gameManager.Initialise();
            stateManager.Initialise();
        }
    }
}
