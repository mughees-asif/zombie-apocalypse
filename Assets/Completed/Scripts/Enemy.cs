using UnityEngine;
using System.Collections;

namespace Completed
{
    public class Enemy : MovingObject
    {
        // Enemy damage points
        public int enemyDamage;

        // Enemy striking action
        private Animator animator;

        private Transform target;

        private bool skipMove;


        // Initialise enemy
        protected override void Start()
        {
            GameManager.instance.AddEnemyToList(this);
            animator = GetComponent<Animator>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            base.Start();
        }


        // Movement functionality for the enemy
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            if (skipMove)
            {
                skipMove = false;
                return;
            }

            base.AttemptMove<T>(xDir, yDir);
            skipMove = true;
        }

        // Movement of the enemy
        public void MoveEnemy()
        {
            int xDir = 0;
            int yDir = 0;

            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
                yDir = target.position.y > transform.position.y ? 1 : -1;
            else
                xDir = target.position.x > transform.position.x ? 1 : -1;

            AttemptMove<Player>(xDir, yDir);
        }


        // Strike the player if near the enemy
        protected override void OnCantMove<T>(T component)
        {
            Player hitPlayer = component as Player;
            hitPlayer.LoseFood(enemyDamage);
            animator.SetTrigger("enemyAttack");
        }
    }
}
