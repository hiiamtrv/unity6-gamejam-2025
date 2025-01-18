using UnityEngine;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float initialReputation;

        private float timer;
        private float reputation;

        private static bool isPlaying;
        public static bool IsPlaying => isPlaying;

        private void Start()
        {
            Reset();
        }

        public void Reset()
        {
            reputation = initialReputation;
            isPlaying = true;
        }

        private void LateUpdate()
        {
            if (isPlaying)
            {
                if (reputation <= 0)
                {
                    Debug.Log("Game Over: No reputation");
                    isPlaying = false;
                    return;
                }
            }
        }

        public void AddReputation(float reputation)
        {
            this.reputation += reputation;
        }
    }
}