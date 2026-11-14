using UnityEngine;
using UnityEngine.InputSystem;

namespace Centipede
{
    public class Player : MonoBehaviour
    {

        private int xpos = 0;
        private int ypos = 0;

        private GameManager gameManager;
        private GridManager gridManager;


        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gridManager = FindAnyObjectByType<GridManager>();
        }
        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                gridManager.DumpGrid();
            }
        }
        public void MovePlayer()
        {
            gridManager.Set(GameEntity.Empty, xpos, ypos);

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector3 gridPos = FindAnyObjectByType<GridManager>().GridPos(mouseWorldPos);

            // bound to lower 12 rows
            if (gridPos.y < 18) gridPos.y = 18;
            if (gridPos.y > 29) gridPos.y = 29;
            if (gridPos.x <  0) gridPos.x = 0;
            if (gridPos.x > 29) gridPos.x = 29;

            // check for collision
            if (gridManager.Get(Mathf.FloorToInt(gridPos.x), Mathf.FloorToInt(gridPos.y)) != GameEntity.Empty) 
            {
                Debug.Log($"x:{gridPos.x}, y:{gridPos.y} Boink Boink!");
                gridManager.Set(GameEntity.Player, xpos, ypos);
                if (gridManager.Get(Mathf.FloorToInt(gridPos.x), Mathf.FloorToInt(gridPos.y)) == GameEntity.CentipedeHead)
                {
                    gameManager.DestroyPlayer();
                }
                return;
            }

            // update grid
            xpos = Mathf.FloorToInt(gridPos.x);
            ypos = Mathf.FloorToInt(gridPos.y);
            gridManager.Set(GameEntity.Player, xpos, ypos);

            // convert to realword coordinates
            gridPos.y = 29 - gridPos.y;
            transform.position = gridPos; 
        }
    }
}
