using System.Collections;
using UnityEngine;

namespace Centipede
{
    public class Centipede : MonoBehaviour
    {
        [SerializeField] private float speed = 0.2f;
        [SerializeField] private Transform spriteTransform;
        [SerializeField] private GameObject bodySprite;
        [SerializeField] private GameObject headSprite;


        private int xpos = 0;
        private int ypos = 0;

        private GameManager gameManager;
        private GridManager gridManager;

        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gridManager = FindAnyObjectByType<GridManager>();
        }
        public IEnumerator Walk(float speed=0.1f)
        {
            this.speed = speed;
            for (int y = 0; y < 30; y+=2)
            {
                yield return StartCoroutine(WalkRight(y));
                yield return StartCoroutine(WalkLeft(y+1));
            }
        }
        private IEnumerator WalkRight(int y)
        {
            FaceRight();
            gridManager.Set(GameEntity.Empty, xpos, ypos); 
            ypos = y;
            for (int x = xpos; x < 30; x++)
            {
                // check if we can go to a spot
                if (gridManager.Get(x,y)==GameEntity.Mushroom)
                {
                    break; // we couldn't, abandon going right
                }
                if (gridManager.Get(x,y)== GameEntity.Player)
                {
                    // hit a player
                    gameManager.DestroyPlayer();
                }
                // remove yourself from the grid
                gridManager.Set(GameEntity.Empty, xpos, ypos); 
                xpos = x;
                transform.position = new Vector3(x, 29-y, 0);
                gridManager.Set(GameEntity.CentipedeHead, xpos, ypos); 
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        private IEnumerator WalkLeft(int y)
        {
            FaceLeft();
            gridManager.Set(GameEntity.Empty, xpos, ypos); 
            ypos = y;
            for (int x = xpos; x >= 0; x--)
            {
                if (gridManager.Get(x,y)==GameEntity.Mushroom)
                {
                    break; //boink
                }
                if (gridManager.Get(x, y) == GameEntity.Player)
                {
                    // hit a player
                    gameManager.DestroyPlayer();
                }
                gridManager.Set(GameEntity.Empty, xpos, ypos); 
                xpos = x;
                transform.position = new Vector3(x, 29-y, 0);
                gridManager.Set(GameEntity.CentipedeHead, xpos, ypos); 
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        private void FaceLeft()
        {
                spriteTransform.eulerAngles = new Vector3(spriteTransform.rotation.x, 
                    spriteTransform.rotation.y, 
                    0);
        }
        private void FaceRight()
        {
                spriteTransform.eulerAngles = new Vector3(spriteTransform.rotation.x, 
                    spriteTransform.rotation.y, 
                    180);
        }
    }
}
