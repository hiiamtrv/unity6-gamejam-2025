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
    [SerializeField] private float freezeTime = 5f;
    [SerializeField] private GameObject bubble;
    [SerializeField] private List<GameObject> spawners; 
    [SerializeField] private int bubbleCount = 6;
    [SerializeField] private List<int> listPosition = new List<int>();
    [SerializeField] private List<GameObject> list = new List<GameObject>();

    private float timer = 0;
    void Start()
    {
        bubbleCount = spawners.Count;
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
            newBubble.transform.position = new Vector2(spawners[listPosition[i]].transform.position.x, spawners[listPosition[i]].transform.position.y);
        }
    }

    public void AddBubble(GameObject bubble)
    {
        list.Add(bubble);
    }
    public void RemoveBubble(GameObject bubble)
    {
        list.Remove(bubble);
    }

    public void MergeBubble(GameObject bubble1, GameObject bubble2)
    {
        Bubble bubble1Component = bubble1.GetComponent<Bubble>();
        Bubble bubble2Component = bubble2.GetComponent<Bubble>();
        int level1 = bubble1Component.Level;
        int level2 = bubble2Component.Level;
        int newLevel = level1 + level2;
        GameObject newBubble = Instantiate(bubble);
        Bubble newComponent = newBubble.GetComponent<Bubble>();
        newComponent.Level = newLevel;
        newBubble.transform.position = (bubble1.transform.position + bubble2.transform.position) / 2;

        int bubble1ColorIndex = bubble1Component.GetColorIndex();
        int bubble2ColorIndex = bubble2Component.GetColorIndex();
        if(bubble2ColorIndex < bubble1ColorIndex) 
        {
            int temp = bubble1ColorIndex;
            bubble1ColorIndex = bubble2ColorIndex;
            bubble2ColorIndex = temp;
        }
        if(bubble2ColorIndex - bubble1ColorIndex > 6)
        {
            bubble1ColorIndex += 12;
        }
        newComponent.SetColorIndex(((bubble1ColorIndex + bubble2ColorIndex)/2) % 12);
        bubble1Component.Pop();
        bubble2Component.Pop();
    }

    public void FreezeBubble()
    {
        foreach (var item in list)
        {
            item.GetComponent<Bubble>().Freeze(freezeTime);
        }
    }
}





