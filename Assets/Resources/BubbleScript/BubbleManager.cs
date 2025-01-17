using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    // singleton pattern
    #region singleton
    private static BubbleManager _instance;
    public static BubbleManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [SerializeField] private float timeToSpawn = 4f;
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private const int bubbleCount = 6;
    [SerializeField] private List<int> listPosition = new List<int> ();
    [SerializeField] private List<GameObject> list = new List<GameObject> ();
    private float timer = 0;
    void Start()
    {
        for (int i = 0; i < bubbleCount; i++)
        {
            listPosition.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToSpawn)
        {
            SpawnRandomBubble();
            timer = 0;
        }
    }

    void ListShuffle()
    {
        for (int i = listPosition.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i);
            int temp = listPosition[i];
            listPosition[i] = listPosition[j];
            listPosition[j] = temp;
        }
    }

    void SpawnRandomBubble()
    {
        int randomAmountBubble = Random.Range(0, bubbleCount);
        ListShuffle();
        for (int i = 0; i < randomAmountBubble; i++)
        {
            GameObject newBubble = Instantiate(bubble);
            newBubble.transform.position = new Vector2((pointB.transform.position.x - pointA.transform.position.x) / bubbleCount * listPosition[i] + pointA.transform.position.x,pointA.transform.position.y);
            list.Add(newBubble);
        }
    }

    public void RemoveBubble(GameObject bubble)
    {
        list.Remove(bubble);
    }

    public void MergeBubble(GameObject bubble1, GameObject bubble2)
    {
        // merge 2 bubble
        int level1 = bubble1.GetComponent<Bubble>().Level;
        int level2 = bubble2.GetComponent<Bubble>().Level;
        int newLevel = level1 + level2;
        GameObject newBubble = Instantiate(bubble);
        newBubble.GetComponent<Bubble>().Level = newLevel;
        newBubble.transform.position = (bubble1.transform.position + bubble2.transform.position)/2;
        Destroy(bubble1);
        Destroy(bubble2);
    }
}
