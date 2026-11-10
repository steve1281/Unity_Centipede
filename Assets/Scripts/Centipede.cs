using System.Collections;
using UnityEngine;

namespace Centipede
{
    public class Centipede : MonoBehaviour
    {
        [SerializeField] private float speed = 0.2f;

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
            for (int x = 0; x < 30; x++)
            {
                transform.position = new Vector3(x, 29-y, 0);
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
        private IEnumerator TestWalkLeft(int y)
        {
            for (int x = 29; x >= 0; x--)
            {
                transform.position = new Vector3(x, 29-y, 0);
                yield return (new WaitForSecondsRealtime(speed));
            }
        }
    }
}
