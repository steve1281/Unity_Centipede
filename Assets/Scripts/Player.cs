using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Centipede
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private Transform cannon;
        [SerializeField] private GameObject bulletPrefab;

        private GameObject bullet=null;


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
                //gameManager.AddSpider(0,25);
                //gridManager.DumpGrid();
                //gameManager.TestKillCentipedeHeads();
            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (bullet == null)
                {
                    bullet = Instantiate (bulletPrefab);
                    bullet.transform.position = cannon.transform.position;
                }
            }
        }
        public void MovePlayer()
        {
            gridManager.Set(GameEntity.Empty, xpos, ypos, null);

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector3 gridPos = FindAnyObjectByType<GridManager>().GridPos(mouseWorldPos);

            // bound to lower 12 rows
            if (gridPos.y < 18) gridPos.y = 18;
            if (gridPos.y > 29) gridPos.y = 29;
            if (gridPos.x <  0) gridPos.x = 0;
            if (gridPos.x > 29) gridPos.x = 29;

            // check for collision
            GameEntity temp = gridManager.Get(Mathf.FloorToInt(gridPos.x), Mathf.FloorToInt(gridPos.y));
            
            if (temp != GameEntity.Empty) 
            {
                Debug.Log($"x:{gridPos.x}, y:{gridPos.y} Boink Boink! Hit {temp}");
                gridManager.Set(GameEntity.Player, xpos, ypos, gameObject);
                if (temp == GameEntity.CentipedeHead)
                {
                    gameManager.DestroyPlayer();
                }

                return;
            }

            // update grid
            xpos = Mathf.FloorToInt(gridPos.x);
            ypos = Mathf.FloorToInt(gridPos.y);
            gridManager.Set(GameEntity.Player, xpos, ypos, gameObject);

            // convert to realword coordinates
            gridPos.y = 29 - gridPos.y;
            transform.position = gridPos; 
        }
    }
}
