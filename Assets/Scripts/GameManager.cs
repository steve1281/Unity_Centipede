using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject spacerPrefab;


        private static GameManager instance;
        public static GameManager Instance { get => instance; private set => instance = value; } 

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            /* add code to reload here */
            DrawTestGrid();
        }
        
        private void DrawTestGrid()
        {
            // code will draw a random pattern, with a smiley face.
            // check shows us that our 30x30 grid is oriented correctly.

            HashSet<Vector2Int> skipList = new HashSet<Vector2Int>();

            // Left Eye (a 2x3 block of empty cells)
            skipList.Add(new Vector2Int(8, 19)); skipList.Add(new Vector2Int(9, 19));
            skipList.Add(new Vector2Int(8, 18)); skipList.Add(new Vector2Int(9, 18));
            skipList.Add(new Vector2Int(8, 17)); skipList.Add(new Vector2Int(9, 17));

            // Right Eye (a 2x3 block of empty cells)
            skipList.Add(new Vector2Int(20, 19)); skipList.Add(new Vector2Int(21, 19));
            skipList.Add(new Vector2Int(20, 18)); skipList.Add(new Vector2Int(21, 18));
            skipList.Add(new Vector2Int(20, 17)); skipList.Add(new Vector2Int(21, 17));

            // The Smile (Bottom flat part)
            for (int x = 10; x <= 19; x++) skipList.Add(new Vector2Int(x, 7));

            // Left smile curve
            skipList.Add(new Vector2Int(9, 8)); skipList.Add(new Vector2Int(8, 9));
            // Right smile curve
            skipList.Add(new Vector2Int(20, 8)); skipList.Add(new Vector2Int(21, 9));

            for (int x = 0; x < 30; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    // Create our coordinate key
                    Vector2Int currentPos = new Vector2Int(x, y);

                    // Check if this coordinate is part of the smiley face cutout
                    if (skipList.Contains(currentPos))
                    {
                        continue; // Skip
                    }

                    // create spacer prefab
                    GameObject g = Instantiate(spacerPrefab);
                    g.transform.position = new Vector3(x, y, 0);

                    // set color to some random color
                    SpriteRenderer spriteSR = g.GetComponentInChildren<SpriteRenderer>();
                    spriteSR.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.7f, 1f);
                }
            }
        }
    }
}
