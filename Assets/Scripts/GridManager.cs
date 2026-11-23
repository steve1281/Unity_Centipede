using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Centipede
{
    public class GridManager : MonoBehaviour
    {
        private const int Width = 30;
        private const int Height = 30;

        [Header("Prefabs")]
        [SerializeField] private GameObject spacerPrefab;

        private GameEntity[,] gridData = new GameEntity[Width, Height];
        private GameObject[,] gridGoData = new GameObject[Width, Height];
        public void Set(GameEntity gameEntity, int x, int y, GameObject go) //=null)
        {
            if (gridGoData[x, y] != null)
            {
                //Destroy(gridGoData[x, y]);
                //return;
            }

            if (x < 0) return;
            if (y < 0) return;
            if (x > Width  - 1) return;
            if (x > Height - 1) return;

            gridData[x, y] = gameEntity; 
            gridGoData[x, y] = go;
            //if (go != null) Debug.Log($"GridManager::Set added a {go.name}");

        }
        public GameEntity Get(int x, int y)
        {
            if (x < 0) return GameEntity.Empty;
            if (y < 0) return GameEntity.Empty;
            if (x > Width  - 1) return GameEntity.Empty;
            if (x > Height - 1) return GameEntity.Empty;

            return gridData[x, y];
        }
        public GameObject GetGo(int x, int y)
        {
            if (x < 0) return null;
            if (y < 0) return null;
            if (x > Width - 1) return null;
            if (x > Height - 1) return null; 

            return gridGoData[x, y];
        }

        public bool GoContains(GameObject gameEntity)
        {
            return gridGoData.Cast<GameObject>().Contains(gameEntity);
        }

        private void Awake()
        {
            Init();
        }
        public void Init()
        {
            for (int x = 0; x <Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    gridData[x, y] = GameEntity.Empty;
                }
            }
        }

        private Vector3 WorldPos(int x, int y)
        {
            // convert grid pos to realworld position
            return new Vector3(x, 29 - y, 0);
        }
        public Vector3 GridPos(Vector3 worldPos)
        {
            int x = Mathf.FloorToInt(worldPos.x);
            int y = Mathf.FloorToInt(worldPos.y);

            return new Vector3(x, 29 - y, 0);
        }

        private GameEntity MapToEnum(GameObject go)
        {
            if (go == null) return GameEntity.Empty;

            if (go.name[0] == 'C') return GameEntity.CentipedeHead;
            if (go.name[0] == 'M') return GameEntity.Mushroom;
            if (go.name[0] == 'P') return GameEntity.Player;
            if (go.name[0] == 'B') return GameEntity.Bullet;

            return GameEntity.Empty;
        }
        public void DumpGrid()
        {
            StringBuilder sb1 = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    sb1.Append(gridData[x, y].ToString()[0]);
                    //sb.Append(',');
                }
                //sb.Length--;
                sb1.AppendLine();
            }
            Debug.Log(sb1.ToString());

            StringBuilder sb2 = new StringBuilder();

            sb2.Clear();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (gridGoData[x,y] != null) 
                    {
                        sb2.Append(gridGoData[x, y].ToString()[0]);
                    } else
                    {
                        sb2.Append("E");
                    }
                    //sb.Append(',');
                }
                //sb.Length--;
                sb2.AppendLine();
            }
            Debug.Log(sb2.ToString());

            if (!sb1.Equals(sb2))
            {
                Debug.Log($"NOT THE SAME. ");
            }
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
            //HashSet<Vector3> skipList = TestDrawSmileyData();
            HashSet<Vector3> skipList = TestDrawSmileyData2();

            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    Vector3 currentPos = WorldPos(x, y);

                    if (skipList.Contains(new Vector3(x,y,0))) { continue; }

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
        private HashSet<Vector3> TestDrawSmileyData2()
        {
            HashSet<Vector3> smileCoordinates = new HashSet<Vector3>();

            // === EYES (2x2 Blocks) ===
            // Left Eye
            smileCoordinates.Add(new Vector3(8, 9, 0)); smileCoordinates.Add(new Vector3(9, 9, 0));
            smileCoordinates.Add(new Vector3(8, 10, 0)); smileCoordinates.Add(new Vector3(9, 10, 0));

            // Right Eye
            smileCoordinates.Add(new Vector3(20, 9, 0)); smileCoordinates.Add(new Vector3(21, 9, 0));
            smileCoordinates.Add(new Vector3(20, 10, 0)); smileCoordinates.Add(new Vector3(21, 10, 0));


            // === MOUTH CURVE (Left to Right) ===
            // Left Upturn
            smileCoordinates.Add(new Vector3(5, 15, 0));
            smileCoordinates.Add(new Vector3(6, 16, 0));
            smileCoordinates.Add(new Vector3(7, 17, 0));
            smileCoordinates.Add(new Vector3(8, 18, 0));
            smileCoordinates.Add(new Vector3(9, 19, 0));
            smileCoordinates.Add(new Vector3(10, 20, 0));
            smileCoordinates.Add(new Vector3(11, 21, 0));

            // Bottom Base
            smileCoordinates.Add(new Vector3(12, 22, 0)); smileCoordinates.Add(new Vector3(13, 22, 0));
            smileCoordinates.Add(new Vector3(14, 23, 0)); smileCoordinates.Add(new Vector3(15, 23, 0));
            smileCoordinates.Add(new Vector3(16, 22, 0)); smileCoordinates.Add(new Vector3(17, 22, 0));

            // Right Upturn
            smileCoordinates.Add(new Vector3(18, 21, 0));
            smileCoordinates.Add(new Vector3(19, 20, 0));
            smileCoordinates.Add(new Vector3(20, 19, 0));
            smileCoordinates.Add(new Vector3(21, 18, 0));
            smileCoordinates.Add(new Vector3(22, 17, 0));
            smileCoordinates.Add(new Vector3(23, 16, 0));
            smileCoordinates.Add(new Vector3(24, 15, 0));

            return smileCoordinates;

        }
    }


    public enum GameEntity { 
        Empty, 
        Mushroom, 
        Spider,
        PoisonMushroom,
        Player,
        CentipedeHead,
        CentipedeBody,
        Bullet
    }
}
