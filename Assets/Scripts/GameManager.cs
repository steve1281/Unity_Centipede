using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {

        private GridManager gridManager;
        private Player player;
        private List<Centipede> centipedes = new List<Centipede>();



        [Header ("Centipedes")]
        [SerializeField] private Centipede centipedePrefab;
        [SerializeField] private int centipedeCount = 0;
        [SerializeField] private float centipedeSpawnDelay = 0.1f;

        [Header("Mushrooms")]
        [SerializeField] private GameObject mushroomPrefab;
        [SerializeField] private int mushroomCount = 1;
        private List<GameObject> mushrooms = new List<GameObject>();


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
            //gridManager.TestMouseClickXY();
            player.MovePlayer();
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
            GenerateMushrooms();
            SpawnMushrooms();

            GenerateCentipedes();
            StartCoroutine(SpawnCentipedes());
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
            int x = Random.Range(0, 30);
            int y = Random.Range(0, 12);
            mushroom.transform.position = new Vector3(x, 29-y, 0);
        }
        private IEnumerator SpawnCentipedes()
        {
            for (int i = 0; i < centipedes.Count; i++)
            {
                SpawnCentipede(centipedes[i]);
                yield return new WaitForSecondsRealtime(centipedeSpawnDelay);
            }
        }
        private void SpawnCentipede(Centipede centipede)
        { 
            StartCoroutine(centipede.TestWalkTheScreen());
        }
        private void GenerateCentipedes()
        {
            for (int i = 0; i < centipedeCount; i++)
            {
                Centipede centipede = Instantiate<Centipede>(centipedePrefab);
                centipede.transform.position = new Vector3(-1, 29, 0);
                centipedes.Add(centipede);
            }
        }
 
    }
}
