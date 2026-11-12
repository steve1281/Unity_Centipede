using System.Collections;
using UnityEngine;

namespace Centipede
{
    public class Centipede : MonoBehaviour
    {
        [SerializeField] private float speed = 0.2f;
        [SerializeField] private Transform spriteTransform;

        private int xpos = 0;

        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        public IEnumerator TestWalkTheScreen()
        {
            for (int y = 0; y < 30; y+=2)
            {
                yield return StartCoroutine(TestWalkRight(y));
                yield return StartCoroutine(TestWalkLeft(y+1));
            }
        }
        private IEnumerator TestWalkRight(int y)
        {
            LookRight();
            for (int x = xpos; x < 30; x++)
            {
                xpos = x;
                if (gameManager.GridGet(x,y)==GameEntity.Mushroom)
                {
                    xpos--;
                    break; //boink
                }
                transform.position = new Vector3(x, 29-y, 0);
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        private IEnumerator TestWalkLeft(int y)
        {
            LookLeft();
            for (int x = xpos; x >= 0; x--)
            {
                xpos = x;
                if (gameManager.GridGet(x,y)==GameEntity.Mushroom)
                {
                    xpos++;
                    break; //boink
                }
                transform.position = new Vector3(x, 29-y, 0);
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        private void LookLeft()
        {
                spriteTransform.eulerAngles = new Vector3(spriteTransform.rotation.x, 
                    spriteTransform.rotation.y, 
                    0);
        }
        private void LookRight()
        {
                spriteTransform.eulerAngles = new Vector3(spriteTransform.rotation.x, 
                    spriteTransform.rotation.y, 
                    180);
        }
    }
}
