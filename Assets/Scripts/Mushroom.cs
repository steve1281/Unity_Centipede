using UnityEngine;

namespace Centipede
{
    public class Mushroom : MonoBehaviour
    {
        private int hitCount = 0;
        private GameManager gameManager;
        private GridManager gridManager;
        [SerializeField] private Animator anim;

        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gridManager = FindAnyObjectByType<GridManager>();
        }
        public void Hit()
        {
            int posx = Mathf.FloorToInt(transform.position.x);
            int posy = Mathf.FloorToInt(transform.position.y); 

            Debug.Log("Mushroom::Hit called.");
            hitCount++;

            if (hitCount == 1)
            {
                anim.SetBool("GoTwo", true);
            }
            else if (hitCount == 2)
            {
                anim.SetBool("GoThree", true);

            } else if (hitCount == 3)
            {
                gridManager.Set(GameEntity.Empty, posx, 29 - posy, null);
                gameManager.UpdateScore(1);
                Destroy(gameObject);
            }
        }
    }
}
