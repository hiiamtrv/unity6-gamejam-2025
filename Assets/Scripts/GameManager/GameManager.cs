using UnityEngine;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int initialReputation;
        public int maxReputation;
        public int reputation;

        public static bool isPlaying;

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
                    UIManager.Instance.ShowGameOverPanel();
                    return;
                }
            }
        }

        public void AddReputation(int reputation)
        {
            this.reputation += reputation;
            UIManager.Instance.UpdateReputationScores(reputation);
        }
    }
}