using System.Collections;
using UnityEngine;

namespace Centipede
{
    public class Centipede : MonoBehaviour, IHittable
    {
        [SerializeField] private float speed = 0.2f;
        [SerializeField] private Transform spriteTransform;
        [SerializeField] private GameObject bodySprite;
        [SerializeField] private GameObject headSprite;

        [Header("Centipede")]
        [SerializeField] private Centipede centipedePrefab;

        private int xpos = 0;
        private int ypos = 1;
        public int direction = 1; // -1 is LEFT, 1 is RIGHT

        private int bodyCount = 0;
        private Centipede nextSegment = null;

        private GameManager gameManager;
        private GridManager gridManager;
        public Centipede GetNextSegment()
        {
            return nextSegment;
        }
        public void SetBodyCount(int bodyCount)
        {
            this.bodyCount = bodyCount;
            if (bodyCount >0)
            {
                Centipede centipede = Instantiate<Centipede>(centipedePrefab, new Vector3(xpos - 1, ypos, 0), Quaternion.identity, gameManager.GetGameElements());
                //centipede.transform.position = new Vector3(xpos-1, ypos, 0);
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

        public void Promote( )
        {
            //Debug.Log("Centipede::Promote was called.");
            bodySprite.SetActive(false);
            headSprite.SetActive(true);
            direction *= -1;
            if (direction == -1) ypos--;
            StartCoroutine(Walk());
        }
        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gridManager = FindAnyObjectByType<GridManager>();
        }
        public IEnumerator Walk(float speed=0.1f)
        {
            this.speed = speed;
            for (int y = ypos; y < 30; y+=2)
            {
                if (direction == 1)  // right 
                    yield return StartCoroutine(WalkRight(y));
                if (direction == -1) // left
                    yield return StartCoroutine(WalkLeft(y + 1));
            }
        }
        private IEnumerator WalkRight(int y)
        {
            direction = -1;
            FaceRight();
            gridManager.Set(GameEntity.Empty, xpos, ypos, null); 
            for (int x = xpos; x < 30; x++)
            {
                //Debug.Log($"You are about to hit {gridManager.Get(x,y)}");
                // check if we can go to a spot
                if (gridManager.Get(x,y)==GameEntity.Mushroom)
                {
                    Debug.Log("abandon going right.");
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
            direction = 1;
            FaceLeft();
            gridManager.Set(GameEntity.Empty, xpos, ypos, null); 
            for (int x = xpos; x >= 0; x--)
            {
                if (gridManager.Get(x,y)==GameEntity.Mushroom)
                {
                    Debug.Log("abandon going left.");
                    break; // abandon doing left
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
            gridManager.Set(GameEntity.Empty, xpos, ypos, null);  // erase
            if (nextSegment != null) nextSegment.Move(xpos, ypos); // call child, if any
            ypos = y;
            xpos = x;
            transform.position = new Vector3(xpos, 29 - ypos, 0);
            gridManager.Set(GameEntity.CentipedeHead, xpos, ypos, gameObject);
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

        public void Hit()
        {

            //Debug.Log($"Centipede::Hit called. Headshot: {headSprite.activeSelf}");
            // update score
            if (headSprite.activeSelf)
            {
                gameManager.UpdateScore(100); // headshot
            } else
            {
                gameManager.UpdateScore(10); // bodyshot
            }

            int posx = Mathf.FloorToInt(transform.position.x);
            int posy = Mathf.FloorToInt(transform.position.y);

            gridManager.Set(GameEntity.Empty, posx, 29 - posy, null);
            Destroy(gameObject);

            gameManager.AddMushroom(posx, posy, nextSegment);
        }
    }
}
