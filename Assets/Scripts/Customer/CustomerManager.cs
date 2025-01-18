using DG.Tweening;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [System.Serializable]
    public struct CustomerPos
    {
        public Transform pos;
        public GameObject customerGameObject;
        public bool isTaking;
    }

    [SerializeField] CustomerPos[] customerPosList;
    [SerializeField] Transform intialPos;
    [SerializeField] GameObject customerPrefab;

    private float timeToSpawnMax = 3f;
    private float timeToSpawn = 0;

    private static CustomerManager instance;
    public static CustomerManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    // private void Update()
    // {
    //     if (timeToSpawn < timeToSpawnMax)
    //     {
    //         timeToSpawn += Time.deltaTime;
    //         if (timeToSpawn >= timeToSpawnMax)
    //         {
    //             SpawnCustomer();
    //             timeToSpawn = 0;
    //         }
    //     }
    // }

    public void SpawnCustomer(int[] requestColorIdxs)
    {
        for (int i = 0; i < customerPosList.Length; i++)
        {
            if (!customerPosList[i].isTaking)
            {
                GameObject customerSpawned = Instantiate(customerPrefab, intialPos.position, Quaternion.identity);
                int randIdx = Random.Range(0, requestColorIdxs.Length);
                customerSpawned.SendMessage("CustomerConfigure", requestColorIdxs[randIdx],
                    SendMessageOptions.DontRequireReceiver);

                // Create a sequence for movement and swaying
                Sequence sequence = DOTween.Sequence();

                // Move to the target position
                sequence.Append(customerSpawned.transform.DOMove(customerPosList[i].pos.position, 2f)
                    .SetEase(Ease.Linear)); // Linear movement

                // Add a swaying effect (rock back and forth as they move)
                sequence.Join(customerSpawned.transform.DORotate(new Vector3(0, 0, 15), 0.25f) // Rotate to 15 degrees
                    .SetLoops(-1, LoopType.Yoyo) // Sway back and forth
                    .SetEase(Ease.InOutSine)); // Smooth easing for sway

                customerPosList[i].isTaking = true;
                customerPosList[i].customerGameObject = customerSpawned;
                break;
            }
        }
    }

    public void RemoveCustomer(GameObject gameObject)
    {
        for (int i = 0; i < customerPosList.Length; i++)
        {
            if (customerPosList[i].customerGameObject == gameObject)
            {
                Destroy(customerPosList[i].customerGameObject);
                customerPosList[i].isTaking = false;
            }
        }
    }
}