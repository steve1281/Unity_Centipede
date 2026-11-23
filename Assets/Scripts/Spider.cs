using UnityEngine;

namespace Centipede
{
    public class Spider : MonoBehaviour, IHittable
    {

        private GameManager gameManager;
        private GridManager gridManager;
        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gridManager = FindAnyObjectByType<GridManager>();
        }

        private int posx = 0;
        private int posy = 0;

        public float speed = 8f;

        // Keep spider in the lower player zone (rows 1 to 10, columns 1 to 28)
        public Vector2Int minBounds = new Vector2Int(1, 1);
        public Vector2Int maxBounds = new Vector2Int(28, 10);

        private Vector3 targetPos;
        private int horizontalDir = 1; // 1 = moving right, -1 = moving left

        void Start()
        {
            //Move(transform.position);
            /*
            posx = Mathf.FloorToInt(transform.position.x);
            posy = Mathf.FloorToInt(transform.position.y);
            gridManager.Set(GameEntity.Spider, posx, posy, gameObject);
            transform.position = new Vector3(posx, posy, 0);
            */

            // Pick initial left-to-right or right-to-left direction based on spawn side
            horizontalDir = transform.position.x < 15f ? 1 : -1;
            PickNextWaypoint();
        }

        float prevTime = 0;

        void Update()
        {
            if (prevTime + Time.deltaTime > speed)
            {
                Vector3 newpos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                transform.position = newpos;
                newpos = PickNextWaypoint();
                Move(newpos);
                prevTime = 0;
            } else
            {
                prevTime += Time.deltaTime;
            } 
        }


        private void Move(Vector3 newpos)
        {
            //Debug.Log($"Spider::Move newpos: {newpos.x}, {newpos.y}");
            gridManager.Set(GameEntity.Empty, posx, 29 - posy, null); // remove old position
            posx = Mathf.FloorToInt(newpos.x);
            posy = Mathf.FloorToInt(newpos.y);
            gridManager.Set(GameEntity.Spider, posx, 29 - posy, gameObject);
            transform.position = new Vector3(posx, posy, 0);

        }
        public void Hit()
        {
//            int posx = Mathf.FloorToInt(transform.position.x);
 //           int posy = Mathf.FloorToInt(transform.position.y);
            gameManager.DestroySpider(posx, 29 - posy);
            Debug.Log("Spider::Hit called.");
        }

        private Vector3Int PickNextWaypoint()
        {
            int nextX = Mathf.FloorToInt(transform.position.x);
            int t = Random.Range(minBounds.y, maxBounds.y);
            int nextY=posy; // = Random.Range(minBounds.y, maxBounds.y);
            if (t > posy)
            {
                nextY++; // Random.Range(minBounds.y, maxBounds.y);
            } else
            {
                nextY--; // Random.Range(minBounds.y, maxBounds.y);
            }
            if (nextY < minBounds.y)
            {
                nextY= minBounds.y;
            }
            if (nextY > maxBounds.y)
            {
                nextY= maxBounds.y;
            }
            //Debug.Log($"{nextX},{nextY}");

            // 50% chance to move straight vertically, 50% chance to move diagonally
            if (Random.value > 0.5f)
            {
                nextX += Random.Range(2, 3) * horizontalDir;
            }

            // Bounce off left or right walls
            if (nextX >= maxBounds.x)
            {
                nextX = maxBounds.x;
                horizontalDir = -1;
            }
            else if (nextX <= minBounds.x)
            {
                nextX = minBounds.x;
                horizontalDir = 1;
            }

            targetPos = new Vector3(nextX, nextY, 0f);
            return new Vector3Int(nextX, nextY, 0); // targetPos;
        }
    }
}
