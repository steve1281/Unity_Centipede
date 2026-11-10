using UnityEngine;
using UnityEngine.InputSystem;

namespace Centipede
{
    public class Player : MonoBehaviour
    {
        public void MovePlayer()
        {

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Vector3 gridPos = FindAnyObjectByType<GridManager>().GridPos(mouseWorldPos);

            // bound to lower 12 rows
            if (gridPos.y < 18) gridPos.y = 18;
            if (gridPos.y > 29) gridPos.y = 29;
            if (gridPos.x < 0) gridPos.x = 0;
            if (gridPos.x > 29) gridPos.x = 29;

            // convert to realword coordinates
            gridPos.y = 29 - gridPos.y;

            transform.position = gridPos; 
        }
    }
}
