using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {

        private GridManager gridManager;

        [Header("Player")]
        [SerializeField] private Player playerPrefab;
        [SerializeField] private float minSpawnTime = 0.2f;
        [SerializeField] private float maxSpawnTime = 0.8f;
        private Player player;


        [Header ("Centipedes")]
        [SerializeField] private Centipede centipedePrefab;
        [SerializeField] private int centipedeCount = 0;
        [SerializeField] private float centipedeSpawnDelay = 0.1f;
        private List<Centipede> centipedes = new List<Centipede>();
        //private List<CentipedeData> centipedeData = new List<CentipedeData>();



        [Header("Mushrooms")]
        [SerializeField] private GameObject mushroomPrefab;
        [SerializeField] private int mushroomCount = 1;
        private List<GameObject> mushrooms = new List<GameObject>();

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI ScoreText;
        private int score = 0;

        public void UpdateScore(int amount)
        {
            score = score + amount;
        }


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
        void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
        private void Update()
        {
            ScoreText.text = score.ToString();

            //gridManager.TestMouseClickXY();
            if (player != null)
            {
                player.MovePlayer();
            }

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
            /* grab game objects we need */
            gridManager = FindAnyObjectByType<GridManager>();
            player = FindAnyObjectByType<Player>();

            /* add code to reload here */
            //gridManager.TestDrawGrid();
            //GeneratePlayer();

            GenerateMushrooms();
            SpawnMushrooms();

            GenerateCentipedes();
            StartCoroutine(SpawnCentipedes());
            StartCoroutine(SpawnPlayer());
        }
        private IEnumerator SpawnPlayer()
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSecondsRealtime(waitTime);
            GeneratePlayer();
        }
        private void GeneratePlayer()
        {
            player = Instantiate<Player>(playerPrefab);
            player.transform.position = new Vector3(0, 0, 0);
        }
        private void GenerateMushrooms()
        {
            for (int i = 0; i < mushroomCount; i++)
            {
                GameObject mushroom = Instantiate<GameObject>(mushroomPrefab);
                mushroom.transform.position = new Vector3(-1, 29, 0);
                mushrooms.Add(mushroom);
            }
        }
        private void SpawnMushrooms()
        {
            for (int i = 0;i < mushroomCount;i++)
            {
                SpawnMushroom(mushrooms[i]);
            }
        }
        private void SpawnMushroom(GameObject mushroom)
        {
            int x = Random.Range(1, 29);
            int y = Random.Range(1, 12);
            if (gridManager.Get(x, y) == GameEntity.Empty) 
            { 
                mushroom.transform.position = new Vector3(x, 29 - y, 0);
                gridManager.Set(GameEntity.Mushroom, x, y, mushroom);
            }
        }
        private IEnumerator SpawnCentipedes()
        {
            for (int i = 0; i < centipedes.Count; i++)
            {
                SpawnCentipede(centipedes[i]);
                yield return new WaitForSecondsRealtime(centipedeSpawnDelay);
            }
        }

        private void SpawnCentipede(Centipede cd)
        {
            StartCoroutine(cd.Walk());
        }
        private void GenerateCentipedes()
        {
            for (int i=0; i<centipedeCount; i++) { 
                Centipede centipede = Instantiate<Centipede>(centipedePrefab);
                centipede.transform.position = new Vector3(-1, 28, 0);
                centipede.SetBodyCount(6);
                centipedes.Add(centipede);
            }
        }

        public void DestroyPlayer()
        {
            if (player == null) return;

            Debug.Log("GameManager::DestroyPlayer called.");
            Destroy(player.gameObject);
            StartCoroutine(SpawnPlayer());
        }

        public void TestKillCentipedeHeads()
        {
            List<Centipede> temp = new List<Centipede>();
             
            for (int i = 0; i < centipedes.Count; i++)
            {
                Centipede c = centipedes[i];
                if (c.GetNextSegment() != null)
                {
                    c.GetNextSegment().direction = c.direction;  
                    temp.Add(c.GetNextSegment());
                }
                Destroy(c.gameObject);
            }
            centipedes.Clear();
            for (int i = 0;i < temp.Count;i++)
            {
                centipedes.Add(temp[i]);
                temp[i].Promote();
            }
        }
        public void AddMushroom(int x,  int y, Centipede c=null)
        {
            Debug.Log($"We need a mushroom at {x}, {y}.");

            GameObject mushroom = Instantiate<GameObject>(mushroomPrefab);
            mushroom.transform.position = new Vector3(x, y, 0);
            gridManager.Set(GameEntity.Mushroom, x, 29-y, mushroom);
            mushrooms.Add(mushroom);

            if (c != null)
            {
                c.Promote();
            }
        }
    }
/*
    [System.Serializable]
    public class CentipedeData
    {
        public List<Centipede> centipedes = new List<Centipede>();
    }
*/
}
