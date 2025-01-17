using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float upSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float horizontalOffset = 2f;
    public int Level = 1;

    // 16 colors
    private int colorIndex;
    private float beginTime;

    private void Start()
    {
        if(Level >= 5)
        {
            Destroy(gameObject);
        }
        beginTime = Time.time;
        colorIndex = Random.Range(0, 16);
        gameObject.transform.localScale = new Vector3(Level, Level, 1);
        float offset = Random.Range(-0.5f, 0.5f);
        upSpeed *= Level;
        upSpeed += offset;
        horizontalSpeed += offset;
       
        //GetComponent<SpriteRenderer>().color = BubbleManager.Instance.colors[colorIndex];
    }

    private void Update()
    {
        if(Time.time - beginTime > 10f)         
        {
            Destroy(gameObject);
        }
        //float lên từ từ và di chuyển ngang
        transform.position += Vector3.up * upSpeed * Time.deltaTime;
        transform.position += Vector3.right * Mathf.Cos(Time.time * horizontalSpeed) * horizontalOffset * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble") && this.GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            Debug.Log("Bubble hit Bubble");
            BubbleManager.Instance.MergeBubble(gameObject, collision.gameObject);
        }
        else if(collision.gameObject.CompareTag("Customer"))
        {
            Debug.Log("Bubble hit customer");
        }
    }

    private void OnDestroy()
    {
        BubbleManager.Instance.RemoveBubble(gameObject);
    }
}
