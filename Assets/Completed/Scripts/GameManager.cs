using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Completed
{
    public class GameManager : MonoBehaviour
    {
        // Level completion delay
        public float levelStartDelay = 3f;

        // Delay between each player turn
        public float turnDelay = 0.1f;

        // Starting value for player health                   
        public int playerHealth = 50;

        public static GameManager instance = null;
        [HideInInspector] public bool playersTurn = true;

        // Display current level number
        private Text levelText;

        // Loading image                       
        private GameObject levelImage;

        private BoardManager boardScript;

        // Starting level number
        private int level = 1;

        // Store enemies
        private List<Enemy> enemies;


        // Check if enemies are moving               
        private bool enemiesMoving;

        // Check to prevent the player from moving if the board is not setup fully                  
        private bool doingSetup = true;



        // Setup the board
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            enemies = new List<Enemy>();
            boardScript = GetComponent<BoardManager>();
            InitGame();
        }

        // Scene loader
        void OnLevelWasLoaded(int index)
        {
            level++;
            InitGame();
        }

        //Initializes the game for each level.
        void InitGame()
        {
            doingSetup = true;
            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Level " + level;
            levelImage.SetActive(true);
            Invoke("HideLevelImage", levelStartDelay);
            enemies.Clear();
            boardScript.SetupScene(level);

        }

        void HideLevelImage()
        {
            levelImage.SetActive(false);
            doingSetup = false;
        }

        void Update()
        {
            if (playersTurn || enemiesMoving || doingSetup)
                return;

            // Start moving enemies
            StartCoroutine(MoveEnemies());
        }

        // Add enemies to level 
        public void AddEnemyToList(Enemy script)
        {
            enemies.Add(script);
        }


        // Coroutine to move enemies in sequence
        IEnumerator MoveEnemies()
        {
            enemiesMoving = true;
            yield return new WaitForSeconds(turnDelay);

            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].MoveEnemy();
                yield return new WaitForSeconds(enemies[i].moveTime);
            }

            playersTurn = true;
            enemiesMoving = false;
        }

        // End of the game
        public void GameOver()
        {
            levelText.text = "Your health reached 0. Try again!";
            levelImage.SetActive(true);
            enabled = false;
        }
    }
}

