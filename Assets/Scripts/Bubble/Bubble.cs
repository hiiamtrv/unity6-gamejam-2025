using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float upSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float timeToDestroy = 15f;
    [SerializeField] private float horizontalOffset = 2f;
    [SerializeField] private ColorConfig colorSet;
    public int Level = 1;

    private SpriteRenderer bubbleSprite;

    // 12 colors
    private const int colorCount = 12;
    private int colorIndex;

    public int GetColorIndex() => colorIndex;

    public void SetColorIndex(int index)
    {
        colorIndex = index;
        bubbleSprite.color = colorSet.colors[colorIndex];
    }

    private float beginTime;

    private void Awake()
    {
        // colorIndex = Random.Range(0, colorCount); 
        bubbleSprite = GetComponent<SpriteRenderer>();
        // bubbleSprite.color = colorSet.colors[colorIndex];
    }

    private void Start()
    {
        BubbleManager.Instance.AddBubble(gameObject);

        if (Level >= 5)
        {
            Destroy(gameObject);
        }

        beginTime = Time.time;
        gameObject.transform.localScale = new Vector3(Level, Level, 1);
        float offset = Random.Range(-0.5f, 0.5f);
        upSpeed *= Level;
        upSpeed += offset;
        horizontalSpeed += offset;

        //GetComponent<SpriteRenderer>().color = BubbleManager.Instance.colors[colorIndex];
    }

    private void Update()
    {
        if (Time.time - beginTime > timeToDestroy)
        {
            Pop();
        }

        //float lên từ từ và di chuyển ngang
        transform.position += Vector3.up * upSpeed * Time.deltaTime;
        transform.position +=
            Vector3.right * Mathf.Cos(Time.time * horizontalSpeed) * horizontalOffset * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + " " + collision.gameObject.tag + " collided with " + gameObject.tag);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collided");
        }
        else if (collision.gameObject.CompareTag("Bubble") && GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            BubbleManager.Instance.MergeBubble(gameObject, collision.gameObject);
        }
        else if (collision.isTrigger && !collision.usedByEffector && collision.gameObject.CompareTag("Customer"))
        {
            Debug.Log($"Bubble hit customer {collision.gameObject.name} {colorIndex}");
            collision.gameObject.SendMessage("SubmitColor", colorIndex, SendMessageOptions.DontRequireReceiver);
            Pop();
        }
    }

    public void Freeze(float freezeTime)
    {
        StartCoroutine(FreezeCoroutine(freezeTime));
    }

    public IEnumerator FreezeCoroutine(float freezeTime)
    {
        float currentUpSpeed = upSpeed;
        float currentHorizontalSpeed = horizontalSpeed;
        float currentHorizontalOffset = horizontalOffset;
        upSpeed = 0;
        horizontalSpeed = 0;
        horizontalOffset = 0;
        yield return new WaitForSeconds(freezeTime);
        upSpeed = currentUpSpeed;
        horizontalSpeed = currentHorizontalSpeed;
        horizontalOffset = currentHorizontalOffset;
    }

    public void Pop()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        BubbleManager.Instance.RemoveBubble(gameObject);
    }
}