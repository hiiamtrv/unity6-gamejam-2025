using UnityEngine;
using Random = UnityEngine.Random;

namespace Bubble
{
    public class BubbleGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 generateIntervals;
        [SerializeField] private GameObject prefabBubble;

        private float timer;

        private void Update()
        {
            if (!GameManager.GameManager.IsPlaying) return;
            
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GenerateBubble();
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            timer = Random.Range(generateIntervals.x, generateIntervals.y);
        }

        private void GenerateBubble()
        {
            var newBubble = Instantiate(prefabBubble, transform.position, Quaternion.identity);

            //call bubble to pass bubble config
            newBubble.SendMessage("SetBubbleConfig", SendMessageOptions.DontRequireReceiver);
        }
    }
}