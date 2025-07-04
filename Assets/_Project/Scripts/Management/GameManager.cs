using System.Collections;
using UnityEngine;

namespace ColourMatch
{
    public class GameManager : MonoBehaviourServiceUser
    {
        [Header("References")]
        [SerializeField] private PlayerView playerView;
        
        private GameCamera gameCamera;
        private GameConfigService gameConfigService;
        private PoolingService poolingService;
        private GameplayControllerService gameplayControllerService;
        private PlayerController playerController;

        private DifficultyLevel currentDifficultyLevel;
        
        private Obstacle obstacle;

        private bool isGameRunning = false;
        
        public void Initialise()
        {
            gameCamera = ResolveServiceDependency<GameCamera>();
            gameConfigService = ResolveServiceDependency<GameConfigService>();
            gameplayControllerService = ResolveServiceDependency<GameplayControllerService>();
            poolingService = ResolveServiceDependency<PoolingService>();

            playerController =
                (PlayerController)GameplayControllerFactory.Create(GameplayControllerType.PlayerController, playerView);
            gameplayControllerService.Register(playerController);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GameBeginEvent>(OnGameBegin);
            EventBus.Subscribe<GameCompleteEvent>(OnGameComplete);
            EventBus.Subscribe<LeftButtonClickedEvent>(OnLeftButtonClicked);
            EventBus.Subscribe<RightButtonClickedEvent>(OnRightButtonClicked);
            EventBus.Subscribe<ColourMismatchEvent>(OnEnemyHitPlayer);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameBeginEvent>(OnGameBegin);
            EventBus.Unsubscribe<GameCompleteEvent>(OnGameComplete);
            EventBus.Unsubscribe<LeftButtonClickedEvent>(OnLeftButtonClicked);
            EventBus.Unsubscribe<RightButtonClickedEvent>(OnRightButtonClicked);
            EventBus.Unsubscribe<ColourMismatchEvent>(OnEnemyHitPlayer);
        }
        
        private void FixedUpdate()
        {
            if (!isGameRunning) return;
            UpdateObstacles();
        }

        private void OnGameBegin(GameBeginEvent gameBeginEvent)
        {
            isGameRunning = true;
            currentDifficultyLevel = gameBeginEvent.DifficultyLevel;
            InitialiseGame();
        }

        private void OnGameComplete(GameCompleteEvent gameCompleteEvent)
        {
            isGameRunning = false;
        }

        /// <summary>
        /// Initial setup of the game.
        /// </summary>
        public void InitialiseGame()
        {
            playerView.gameObject.SetActive(true);
            playerView.PlayerRigidbody.gameObject.SetActive(true);
            playerController.Reset();
            
            gameplayControllerService.ResetAll();
            
            SetPlayerPosition();
            SpawnObstacle();
        }

        /// <summary>
        /// Reset the position of the player to their starting position. 
        /// </summary>
        private void SetPlayerPosition()
        {
            var fixedYPosition = Screen.height * 0.33f;
            var playerScreenPosition = new Vector2(Screen.width * 0.5f, fixedYPosition);
            var playerWorldPosition = gameCamera.ScreenPositionToWorldPosition(playerScreenPosition);
            playerView.PlayerRigidbody.position = playerWorldPosition;
        }

        private void OnLeftButtonClicked(LeftButtonClickedEvent leftButtonClickedEvent)
        {
            Logger.BasicLog(this, "Left button input received — changing player colour.", LogChannel.Gameplay);
            playerController.DecrementColour();
        }

        private void OnRightButtonClicked(RightButtonClickedEvent rightButtonClickedEvent)
        {
            Logger.BasicLog(this, "Right button input received — changing player colour.", LogChannel.Gameplay);
            playerController.IncrementColour();
        }

        /// <summary>
        /// Update the enemy, including spawning, and repooling the enemy when it goes off screen.
        /// </summary>
        private void UpdateObstacles()
        {
            if (!obstacle)
            {
                SpawnObstacle();
            }
            else
            {
                var enemyPositionOnScreen = gameCamera.WorldPositionToScreenPosition(obstacle.transform.position);
                if (enemyPositionOnScreen.y < 0)
                {
                    poolingService.Return(PooledObject.Obstacle, obstacle.gameObject);
                    //player.CollisionOccurredOnPlayer -= OnEnemyHitPlayer;
                    SpawnObstacle();
                }
            }
        }

        /// <summary>
        /// Spawn a new obstacle at it's starting position.
        /// </summary>
        private void SpawnObstacle()
        {
            AudioPlayer.ObstacleSpawn();
            var pooledObject = poolingService.Get(PooledObject.Obstacle);
            obstacle = pooledObject.GetComponent<Obstacle>();
            obstacle.AssignObstacleRandomColour();
            obstacle.SetPosition(
                gameCamera.ScreenPositionToWorldPosition(new Vector2(Screen.width * 0.5f, Screen.height))
            );
            
            obstacle.ObstacleSpeed = gameConfigService.GetSpeedByDifficulty(currentDifficultyLevel);
        }

        /// <summary>
        /// Triggered when the enemy hits the player.
        /// </summary>
        /// <param name="playerObject">Player instance which the enemy has hit.</param>
        private void OnEnemyHitPlayer(ColourMismatchEvent colourMismatchEvent)
        {
            StartCoroutine(GameOverRoutine());
        }

        private void DestroyPlayer()
        {
            var playerImpactVFX = poolingService.Get(PooledObject.PlayerImpactVFX);
            playerImpactVFX.transform.position = playerView.PlayerRigidbody.position;
            playerImpactVFX.transform.rotation = Quaternion.identity;
            
            AudioPlayer.PlayerImpact();
            playerView.gameObject.SetActive(false);
        }

        /// <summary>
        /// Coroutine which runs the game over sequence.
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameOverRoutine()
        {
            DestroyPlayer();
            yield return new WaitForSeconds(gameConfigService.GameOverDelay);
            EventBus.Fire(new GameCompleteEvent{});
        }
    }
}