using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Completed
{
    public class Player : MovingObject
    {
        // Restart timer
        public float restartLevelDelay = 1f;

        // Health gained from picking up food		
        public int healthPerFood = 20;

        // Player damage to wall
        public int wallDamage = 5;

        // Health total
        public Text foodText;

        // Player's animator 
        private Animator animator;

        // Player points from the food			
        private int food;

        // Initialise the player
        protected override void Start()
        {
            animator = GetComponent<Animator>();
            food = GameManager.instance.playerHealth;
            foodText.text = "Health: " + food;
            base.Start();
        }

        // Store health level for each new level
        private void OnDisable()
        {
            GameManager.instance.playerHealth = food;
        }

        // Update player instance every frame
        private void Update()
        {
            if (!GameManager.instance.playersTurn) return;

            // Player movement in the x-y directions 
            int horizontal = 0;
            int vertical = 0;

            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            if (horizontal != 0)
            {
                vertical = 0;
            }

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Wall>(horizontal, vertical);
            }
        }

        // Players movement in the game space
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            food--;
            foodText.text = "Health: " + food;
            base.AttemptMove<T>(xDir, yDir);
            //Check if food point total is less than or equal to zero.
            if (food <= 0)
            {
                GameManager.instance.GameOver();
            }
            GameManager.instance.playersTurn = false;
        }

        // Player's interaction with the wall
        protected override void OnCantMove<T>(T component)
        {
            Wall hitWall = component as Wall;
            hitWall.DamageWall(wallDamage);
            animator.SetTrigger("playerChop");
        }

        // Player's interaction with the food and exit
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Exit")
            {
                Invoke("Restart", restartLevelDelay);
                enabled = false;
            }

            else if (other.tag == "Food" || other.tag == "Soda")
            {
                food += healthPerFood;
                foodText.text = "+" + healthPerFood + " Health: " + food;
                other.gameObject.SetActive(false);
            }
        }


        // Reloads the scene
        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        // Enemy attack on the player
        public void LoseFood(int loss)
        {
            animator.SetTrigger("playerHit");
            food -= loss;
            foodText.text = "-" + loss + " Health: " + food;

            if (food <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
    }
}

