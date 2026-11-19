using UnityEngine;
using System.Collections.Generic;

namespace Centipede
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject sprite;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float speed;

        private int posx = 0;
        private int posy = 0;
        private GameManager gameManager;
        private GridManager gridManager;
        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
            gridManager = FindAnyObjectByType<GridManager>();
        }



        private void Start()
        {
            posx = Mathf.FloorToInt(spawnPoint.position.x);
            posy = Mathf.FloorToInt(spawnPoint.position.y);
            gridManager.Set(GameEntity.Bullet, posx, posy, gameObject);
            transform.position = new Vector3(posx, posy, 0);
        }

        float prevTime = 0;
        private void Update()
        {
            GameEntity temp;

            if (prevTime + Time.deltaTime > speed)
            {
                gridManager.Set(GameEntity.Empty, posx, 29-posy, null);
                posy = posy + 1;
                if (posy >= 29)
                {
                    Destroy(gameObject);
                }
                else
                {
                    temp = gridManager.Get(posx, 29-posy);

                    if (temp != GameEntity.Empty && temp != GameEntity.Player && temp != GameEntity.Bullet)
                    {
                        //Debug.Log($"Bullet::Update Hit a: {temp}.");
                        if (temp == GameEntity.Mushroom)
                        {
                            gridManager.GetGo(posx, 29 - posy).GetComponent<Mushroom>().Hit();  
                        } else
                        {
                            gridManager.Set(GameEntity.Empty, posx, 29 - posy, null);
                        }
                        transform.position = new Vector3(posx, posy, 0);
                        Destroy(gameObject);
                    }
                    else
                    {
                        gridManager.Set(GameEntity.Bullet, posx, 29 - posy, gameObject);
                        transform.position = new Vector3(posx, posy, 0);
                    } 
                }
                prevTime = 0;
            } else
            {
                prevTime += Time.deltaTime;
            }

        }


    }
}
