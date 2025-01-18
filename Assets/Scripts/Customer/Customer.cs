using GameManager;
using UnityEngine;
public class Customer : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int _wishColorIndex;
    [SerializeField] private float _countdownTime;
    [SerializeField] private float _minWaiting, _maxWaiting;
    public void CustomerConfigure(int ColorIndex)
    {
        _wishColorIndex = ColorIndex;
    }

    private void Awake()
    {
        _minWaiting = 3f;
        _maxWaiting = 6f;
        _countdownTime = Random.Range(_minWaiting, _maxWaiting);
    }
    private void OnEnable()
    {
        _wishColorIndex = RandomFavouriteColorIndex();
    }
    private int RandomFavouriteColorIndex()
    {
        return Random.Range(0, 17);
    }
    private void Waiting()
    {
        if (_countdownTime > 0)
        {
            _countdownTime -= Time.deltaTime;
            Debug.Log("I'm waiting");
        }
        else
        {
            Debug.Log("Done waiting, leaving");
            Leave();
        }
    }
    private void Satisfied()
    {
        //Show satisfaction
        Debug.Log("I like this");
    }
    private void Angry()
    {
        //Show angry face
        Debug.Log("I dont like this");
        //Deduce reputation
        GameManager gameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        if (gameManager != null)
        {
            gameManager.AddReputation(10); // Add 10 to reputation
            Debug.Log("Added 10 reputation.");
        }
    }
    private void ShowingWishColor()
    {
        //Change ColorIndex to real color
        Debug.Log("My favourite color is" + _wishColorIndex.ToString());
        //Delay;
    }
    private void Update()
    {
        //CustomerAppear
        ShowingWishColor();
        //Waiting
        Invoke("Waiting", 1f);
        //DoneWaiting, leave
    }
    private void Leave() {
        Debug.Log("Left...");
        Destroy(gameObject, 1f);

    }
    private void OnCollisionEnter2D(Collision2D coli2D)
    {
        if (coli2D.collider.CompareTag("Bubble"))
        {
            int bubbleColor = 4;
            //bubbleColor = coli2D.collider.TryGetComponent<Bubble>().GetColorIndex();
            //check mau == _wishColorIndex thi huy customer
            if (bubbleColor == _wishColorIndex)
            {
                Satisfied();
                Leave();
            }
            else //check mau != _wishColorIndex thi goi GameManager - mau va huy customer
            {
                Angry();
            }
            
        }

    }
}
