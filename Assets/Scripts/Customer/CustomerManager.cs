using DG.Tweening;
using DG.Tweening.Core;
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
    public Transform intialPos;
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


                // Move to the target position
                customerSpawned.transform.DOMove(customerPosList[i].pos.position, 4f)
                    .SetEase(Ease.OutCirc)
                    .OnComplete(() =>
                    {
                        customerSpawned.SendMessage("ShowingWishColor", SendMessageOptions.DontRequireReceiver);
                        customerSpawned.GetComponentInChildren<Animator>().Play("Idle");
                    });


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
                // Destroy(customerPosList[i].customerGameObject);
                customerPosList[i].isTaking = false;
            }
        }
    }
}