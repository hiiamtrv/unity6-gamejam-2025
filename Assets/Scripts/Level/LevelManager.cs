using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

namespace Level
{
    [Serializable]
    public struct LevelStage
    {
        public int customerRequired;
        public int[] colorIndexPool;
        public int[] colorRequestPool;
        public int maxBubbles;
        public int maxCustomers;
        public Vector2 delaySpawnBubble;
        public Vector2 delaySpawnCustomer;
    }

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelStage> stages;
        [SerializeField] private Vector2 delaySpawnBubbleUnlimited;
        [SerializeField] private Vector2 delaySpawnCustomerUnlimited;

        public static Action<int> CustomerServed;
        private LevelStage? currentStage;

        private int customerRequired;
        private bool isSpawningBubble;
        private bool isSpawningCustomer;

        private readonly int[] defaultColorPool = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        private void Awake()
        {
            CustomerServed += OnCustomerServed;
            if ((stages?.Count ?? 0) > 0)
            {
                AdvanceStage();
            }
        }

        private void Update()
        {
            if (CanGoNextStage())
            {
                AdvanceStage();
            }

            if (currentStage != null)
            {
                RunStage();
            }
            else
            {
                RunUnlimitedStage();
            }
        }

        private bool CanGoNextStage()
        {
            return currentStage != null && customerRequired <= 0;
        }

        private void AdvanceStage()
        {
            if (stages?.Count == 0)
            {
                currentStage = null;
            }
            else
            {
                currentStage = stages[0];
                stages.RemoveAt(0);
                customerRequired = currentStage.Value.customerRequired;
            }
        }

        private void RunStage()
        {
            if (!isSpawningBubble)
            {
                Debug.Assert(currentStage != null);
                var bubbles = GameObject.FindGameObjectsWithTag("Bubble");
                if (bubbles.Length <= currentStage.Value.maxBubbles)
                {
                    SpawnBubble(currentStage.Value.delaySpawnBubble, currentStage?.colorIndexPool);
                }
            }

            if (!isSpawningCustomer)
            {
                Debug.Assert(currentStage != null);
                var customers = GameObject.FindGameObjectsWithTag("Customer");
                if (customers.Length <= currentStage.Value.maxCustomers)
                {
                    SpawnCustomer(currentStage.Value.delaySpawnCustomer, currentStage?.colorRequestPool);
                }
            }
        }

        private void RunUnlimitedStage()
        {
            if (!isSpawningBubble)
            {
                SpawnBubble(delaySpawnBubbleUnlimited, defaultColorPool);
            }

            if (!isSpawningCustomer)
            {
                SpawnCustomer(delaySpawnCustomerUnlimited, defaultColorPool);
            }
        }

        private void SpawnBubble(Vector2 delayMinMax, int[] colorPool)
        {
            //force kill to confirm next spawn is legit
            StopCoroutine(nameof(IeSpawnBubble));
            isSpawningBubble = true;

            var delay = Random.Range(delayMinMax.x, delayMinMax.y);
            StartCoroutine(IeSpawnBubble(delay, colorPool));
        }

        private void SpawnCustomer(Vector2 delayMinMax, int[] requestColorPool)
        {
            //force kill to confirm next spawn is legit
            StopCoroutine(nameof(IeSpawnCustomer));
            isSpawningCustomer = true;

            var delay = Random.Range(delayMinMax.x, delayMinMax.y);
            StartCoroutine(IeSpawnCustomer(delay, requestColorPool));
        }

        IEnumerator IeSpawnBubble(float delay, int[] colorIndexPool)
        {
            isSpawningBubble = true;
            yield return new WaitForSecondsRealtime(delay);
            BubbleManager.Instance.SpawnRandomBubble(1, colorIndexPool);
            isSpawningBubble = false;
        }

        IEnumerator IeSpawnCustomer(float delay, int[] colorRequestPool)
        {
            isSpawningCustomer = true;
            yield return new WaitForSecondsRealtime(delay);
            CustomerManager.Instance.SpawnCustomer(colorRequestPool);
            isSpawningCustomer = false;
        }

        private void OnCustomerServed(int numClient)
        {
            if (currentStage == null) return;
            customerRequired -= numClient;
        }
    }
}