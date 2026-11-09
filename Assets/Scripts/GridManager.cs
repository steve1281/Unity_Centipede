using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Centipede
{
    public class GridManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject spacerPrefab;

        private Vector3 WorldPos(int x, int y)
        {
            // convert grid pos to realworld position
            return new Vector3(x, 29 - y, 0);
        }
        private Vector3 GridPos(Vector3 worldPos)
        {
            int x = Mathf.FloorToInt(worldPos.x);
            int y = Mathf.FloorToInt(worldPos.y);

            return new Vector3(x, 29 - y, 0);
        }
        public void TestMouseClickXY()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {

                Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
                Vector3 gridPos = GridPos(mouseWorldPos);

                Debug.Log($"Hovering over Matrix Coordinates: [{gridPos.x}, {gridPos.y}]");
            }
        }
        public void TestDrawGrid()
        {
            HashSet<Vector3> skipList = TestDrawSmileyData();

            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    Vector3 currentPos = WorldPos(x, y);

                    if (skipList.Contains(currentPos)) { continue; }

                    GameObject g = Instantiate(spacerPrefab);
                    g.transform.position = currentPos;

                    SpriteRenderer spriteSR = g.GetComponentInChildren<SpriteRenderer>();
                    spriteSR.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.7f, 1f);
                }
            }
        }
        private HashSet<Vector3> TestDrawSmileyData()
        {
            HashSet<Vector3> skipList = new HashSet<Vector3>();

            // Left Eye (a 2x3 block of empty cells)
            skipList.Add(new Vector3(8, 19, 0)); skipList.Add(new Vector3(9, 19, 0));
            skipList.Add(new Vector3(8, 18, 0)); skipList.Add(new Vector3(9, 18, 0));
            skipList.Add(new Vector3(8, 17, 0)); skipList.Add(new Vector3(9, 17, 0));

            // Right Eye (a 2x3 block of empty cells)
            skipList.Add(new Vector3(20, 19, 0)); skipList.Add(new Vector3(21, 19, 0));
            skipList.Add(new Vector3(20, 18, 0)); skipList.Add(new Vector3(21, 18, 0));
            skipList.Add(new Vector3(20, 17, 0)); skipList.Add(new Vector3(21, 17, 0));

            // The Smile (Bottom flat part)
            for (int x = 10; x <= 19; x++) skipList.Add(new Vector3(x, 7, 0));

            // Left smile curve
            skipList.Add(new Vector3(9, 8, 0)); skipList.Add(new Vector3(8, 9, 0));
            // Right smile curve
            skipList.Add(new Vector3(20, 8, 0)); skipList.Add(new Vector3(21, 9, 0));

            return skipList;
        }
    }
}
