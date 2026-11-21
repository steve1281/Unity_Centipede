using UnityEngine;

namespace Centipede
{
    public class Spider : MonoBehaviour
    {
        public float speed = 8f;

        // Keep spider in the lower player zone (rows 1 to 10, columns 1 to 28)
        public Vector2 minBounds = new Vector2(1f, 1f);
        public Vector2 maxBounds = new Vector2(28f, 10f);

        private Vector3 targetPos;
        private int horizontalDir = 1; // 1 = moving right, -1 = moving left

        void Start()
        {
            // Pick initial left-to-right or right-to-left direction based on spawn side
            horizontalDir = transform.position.x < 15f ? 1 : -1;
            PickNextWaypoint();
        }

        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                PickNextWaypoint();
            }
        }

        void PickNextWaypoint()
        {
            float nextX = transform.position.x;
            float nextY = Random.Range(minBounds.y, maxBounds.y);

            // 50% chance to move straight vertically, 50% chance to move diagonally
            if (Random.value > 0.5f)
            {
                nextX += Random.Range(2f, 5f) * horizontalDir;
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
        }
    }
}
