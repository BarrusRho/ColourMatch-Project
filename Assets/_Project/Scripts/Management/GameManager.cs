using System.Collections;
using UnityEngine;

namespace ColourMatch
{
    public class GameManager : MonoBehaviourServiceUser
    {
        private GameCamera gameCamera;
        private PoolingService poolingService;
        
        [Header("Scriptable Objects")] [SerializeField]
        private GameVariablesSO gameVariablesSO;

        [Header("References")]
        [SerializeField] private Player player;
        [SerializeField] private Rigidbody2D playerRigidbody;
        private Obstacle obstacle;

        private DifficultyLevel currentDifficultyLevel;
        private bool isGameRunning = false;
        
        public void Initialise()
        {
            gameCamera = ResolveServiceDependency<GameCamera>();
            poolingService = ResolveServiceDependency<PoolingService>();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<GameBeginEvent>(OnGameBegin);
            EventBus.Subscribe<GameCompleteEvent>(OnGameComplete);
            EventBus.Subscribe<LeftButtonClickedEvent>(OnLeftButtonClicked);
            EventBus.Subscribe<RightButtonClickedEvent>(OnRightButtonClicked);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<GameBeginEvent>(OnGameBegin);
            EventBus.Unsubscribe<GameCompleteEvent>(OnGameComplete);
            EventBus.Unsubscribe<LeftButtonClickedEvent>(OnLeftButtonClicked);
            EventBus.Unsubscribe<RightButtonClickedEvent>(OnRightButtonClicked);
        }

        /// <summary>
        /// Update the physics based components with in the game, namely the player, and enemy. 
        /// </summary>
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
            playerRigidbody.gameObject.SetActive(true);
            player.AssignPlayerRandomColour();
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
            playerRigidbody.position = playerWorldPosition;
        }

        private void OnLeftButtonClicked(LeftButtonClickedEvent leftButtonClickedEvent)
        {
            Logger.BasicLog(this, "Left button input received — changing player colour.", LogChannel.Gameplay);
            AudioPlayer.ChangeColour();
            player.DecrementPlayerColour();
        }

        private void OnRightButtonClicked(RightButtonClickedEvent rightButtonClickedEvent)
        {
            Logger.BasicLog(this, "Right button input received — changing player colour.", LogChannel.Gameplay);
            AudioPlayer.ChangeColour();
            player.IncrementPlayerColour();
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
                    player.CollisionOccurredOnPlayer -= OnEnemyHitPlayer;
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
            player.CollisionOccurredOnPlayer += OnEnemyHitPlayer;

            switch (currentDifficultyLevel)
            {
                case DifficultyLevel.Easy:
                    obstacle.ObstacleSpeed = gameVariablesSO.easyDifficultySpeed;
                    break;
                
                case DifficultyLevel.Medium:
                    obstacle.ObstacleSpeed = gameVariablesSO.mediumDifficultySpeed;
                    break;
                
                case DifficultyLevel.Hard:
                    obstacle.ObstacleSpeed = gameVariablesSO.hardDifficultySpeed;
                    break;
            }
        }

        /// <summary>
        /// Triggered when the enemy hits the player.
        /// </summary>
        /// <param name="playerObject">Player instance which the enemy has hit.</param>
        private void OnEnemyHitPlayer(Player playerObject)
        {
            StartCoroutine(GameOverRoutine());
        }

        private void DestroyPlayer()
        {
            player.CollisionOccurredOnPlayer -= OnEnemyHitPlayer;

            var playerImpactVFX = poolingService.Get(PooledObject.PlayerImpactVFX);
            playerImpactVFX.transform.position = playerRigidbody.position;
            playerImpactVFX.transform.rotation = Quaternion.identity;
            
            AudioPlayer.PlayerImpact();
            player.gameObject.SetActive(false);
        }

        /// <summary>
        /// Coroutine which runs the game over sequence.
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameOverRoutine()
        {
            DestroyPlayer();
            yield return new WaitForSeconds(gameVariablesSO.gameOverDelay);
            EventBus.Fire(new GameCompleteEvent{});
        }
    }
}