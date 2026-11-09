using UnityEngine;
using UnityEngine.SceneManagement;

namespace Centipede
{
    public class GameManager : MonoBehaviour
    {

        private GridManager gridManager;

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
        private void Update()
        {
            gridManager.TestMouseClickXY();
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

            /* add code to reload here */
            gridManager.TestDrawGrid();
        }
 
    }
}
