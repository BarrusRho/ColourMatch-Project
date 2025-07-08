using System.Collections;
using UnityEngine;

namespace ColourMatch
{
    public class GameplayCoordinator : MonoBehaviourServiceUser
    {
        [Header("References")]
        [SerializeField] private PlayerView playerView;
        
        private GameConfigService gameConfigService;
        private GameplaySystemService gameplaySystemService;
        private GameplayControllerService gameplayControllerService;
        private PlayerController playerController;
        private ObstacleController obstacleController;

        private DifficultyLevel currentDifficultyLevel;
        
        public void Initialise()
        {
            gameConfigService = ResolveServiceDependency<GameConfigService>();
            gameplaySystemService = ResolveServiceDependency<GameplaySystemService>();
            gameplayControllerService = ResolveServiceDependency<GameplayControllerService>();

            playerController = (PlayerController)GameplayControllerFactory.Create(GameplayControllerType.PlayerController, playerView);
            gameplayControllerService.Register(playerController);
            gameplaySystemService.RegisterSystem(playerController);
            
            obstacleController = (ObstacleController)GameplayControllerFactory.Create(GameplayControllerType.ObstacleController);
            gameplayControllerService.Register(obstacleController);
            gameplaySystemService.RegisterSystem(obstacleController);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GameBeginEvent>(OnGameBegin);
            EventBus.Subscribe<GameCompleteEvent>(OnGameComplete);
            EventBus.Subscribe<ColourMismatchEvent>(OnEnemyHitPlayer);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameBeginEvent>(OnGameBegin);
            EventBus.Unsubscribe<GameCompleteEvent>(OnGameComplete);
            EventBus.Unsubscribe<ColourMismatchEvent>(OnEnemyHitPlayer);
        }

        private void OnGameBegin(GameBeginEvent gameBeginEvent)
        {
            currentDifficultyLevel = gameBeginEvent.DifficultyLevel;
            InitialiseGameplay();
        }

        private void OnGameComplete(GameCompleteEvent gameCompleteEvent)
        {
        }

        private void InitialiseGameplay()
        {
            playerController.PrepareForGameStart();
            obstacleController.SetDifficulty(currentDifficultyLevel);
            gameplayControllerService.ResetAll();
        }
        
        private void OnEnemyHitPlayer(ColourMismatchEvent colourMismatchEvent)
        {
            StartCoroutine(GameOverRoutine());
        }
        
        private IEnumerator GameOverRoutine()
        {
            playerController.HandlePlayerCollision();
            yield return new WaitForSeconds(gameConfigService.GameOverDelay);
            EventBus.Fire(new GameCompleteEvent{});
        }
    }
}