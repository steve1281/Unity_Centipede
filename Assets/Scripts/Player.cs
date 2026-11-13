using UnityEngine;
using UnityEngine.InputSystem;

namespace Centipede
{
    public class Player : MonoBehaviour
    {

        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        public void MovePlayer()
        {

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector3 gridPos = FindAnyObjectByType<GridManager>().GridPos(mouseWorldPos);

            // bound to lower 12 rows
            if (gridPos.y < 18) gridPos.y = 18;
            if (gridPos.y > 29) gridPos.y = 29;
            if (gridPos.x <  0) gridPos.x = 0;
            if (gridPos.x > 29) gridPos.x = 29;

            // check for collision
            if (gameManager.GridGet((int)gridPos.x, (int)gridPos.y) != GameEntity.Empty) 
            {
                Debug.Log($"x:{gridPos.x}, y:{gridPos.y} Boink Boink!");
                return;
            }

            // convert to realword coordinates
            gridPos.y = 29 - gridPos.y;

            

            transform.position = gridPos; 
        }
    }
}
