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

        [Header("Centipede")]
        [SerializeField] private Centipede centipedePrefab;

        private int xpos = 0;
        private int ypos = 0;

        private int bodyCount = 0;
        private Centipede nextSegment = null;

        private GameManager gameManager;
        private GridManager gridManager;

        public void SetBodyCount(int bodyCount)
        {
            this.bodyCount = bodyCount;
            if (bodyCount >0)
            {
                Centipede centipede = Instantiate<Centipede>(centipedePrefab);
                centipede.transform.position = new Vector3(xpos-1, ypos, 0);
                centipede.SetBodyCount(bodyCount-1);
                centipede.Demote();
                nextSegment = centipede;
            }
        }

        public void Demote()
        {
            bodySprite.SetActive(true);
            headSprite.SetActive(false);
        }

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
                Move(x, y);
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        private IEnumerator WalkLeft(int y)
        {
            FaceLeft();
            gridManager.Set(GameEntity.Empty, xpos, ypos); 
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
                Move(x, y);
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        public void Move(int x, int y)
        {
            gridManager.Set(GameEntity.Empty, xpos, ypos);  // erase
            if (nextSegment != null) nextSegment.Move(xpos, ypos); // call child, if any
            ypos = y;
            xpos = x;
            transform.position = new Vector3(xpos, 29 - ypos, 0);
            gridManager.Set(GameEntity.CentipedeHead, xpos, ypos);
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
