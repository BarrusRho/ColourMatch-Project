using System;
using System.Collections;
using UnityEngine;

namespace ColourMatch
{
    /// <summary>
    /// Game component responsible for the behaviour and logic of the game. 
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private StateManager stateManager;
        
        [Header("Scriptable Objects")] [SerializeField]
        private GameVariablesSO gameVariablesSO;

        [SerializeField] private VFXPrefabsSO vfxPrefabsSO;

        [Header("References")]
        [SerializeField] private AudioManager audioManager;

        [SerializeField] private PoolManager poolManager;

        /// <summary>
        /// GameCamera component used for converting between screen and world coordinates.
        /// </summary>
        [SerializeField] private GameCamera gameCamera;

        /// <summary>
        /// GameHUD component used for providing game UI.
        /// </summary>
        [SerializeField] private GameHUD gameHUD;

        [SerializeField] private Player player;

        /// <summary>
        /// Rigidbody2D component of the player.
        /// </summary>
        [SerializeField] private Rigidbody2D playerRigidbody;

        /// <summary>
        /// The enemy in the game.
        /// </summary>
        private Obstacle obstacle;

        /// <summary>
        /// Delegate triggered when the game is complete.
        /// </summary>
        public Action GameComplete = delegate { };

        private void Update()
        {
            UpdatePlayerColour();
        }

        /// <summary>
        /// Update the physics based components with in the game, namely the player, and enemy. 
        /// </summary>
        private void FixedUpdate()
        {
            UpdateObstacles();
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

        public void SetStateManager(StateManager manager)
        {
            stateManager = manager;
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

        /// <summary>
        /// Update the player colour based on the input.
        /// </summary>
        private void UpdatePlayerColour()
        {
            if (gameHUD.LeftButton.IsButtonClicked)
            {
                audioManager.PlayAudioClip(AudioTag.ChangeColour);
                player.DecrementPlayerColour();
                gameHUD.LeftButton.IsButtonClicked = false;
            }
            else if (gameHUD.RightButton.IsButtonClicked)
            {
                audioManager.PlayAudioClip(AudioTag.ChangeColour);
                player.IncrementPlayerColour();
                gameHUD.RightButton.IsButtonClicked = false;
            }
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
                    obstacle.ReturnObstacleToPool(obstacle);
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
            audioManager.PlayAudioClip(AudioTag.NewObstacleSpawn);
            obstacle = poolManager.GetObstacleFromPool();
            obstacle.AssignObstacleRandomColour();
            obstacle.SetPosition(
                gameCamera.ScreenPositionToWorldPosition(new Vector2(Screen.width * 0.5f, Screen.height))
            );
            player.CollisionOccurredOnPlayer += OnEnemyHitPlayer;

            if (stateManager == null) return;
            switch (stateManager.selectedDifficulty)
            {
                case StateManager.DifficultyLevels.Easy:
                    obstacle.ObstacleSpeed = gameVariablesSO.easyDifficultySpeed;
                    break;
                
                case StateManager.DifficultyLevels.Medium:
                    obstacle.ObstacleSpeed = gameVariablesSO.mediumDifficultySpeed;
                    break;
                
                case StateManager.DifficultyLevels.Hard:
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
            Instantiate(vfxPrefabsSO.playerImpactVFX, playerRigidbody.position, Quaternion.identity);
            audioManager.PlayAudioClip(AudioTag.PlayerImpact);
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
            GameComplete();
        }
    }
}