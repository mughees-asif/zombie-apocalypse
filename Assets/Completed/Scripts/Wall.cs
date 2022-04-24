using UnityEngine;
using System.Collections;

namespace Completed
{
    public class Wall : MonoBehaviour
    {

        // Damage to the wall by the player
        public Sprite dmgSprite;

        // Hit points for the wall
        public int hp = 3;

        private SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        // Player's interaction with a wall component
        public void DamageWall(int loss)
        {
            spriteRenderer.sprite = dmgSprite;
            hp -= loss;

            if (hp <= 0)
                gameObject.SetActive(false);
        }
    }
}
