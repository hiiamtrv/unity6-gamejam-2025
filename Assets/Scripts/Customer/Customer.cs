using GameManager;
using Level;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Values")] [SerializeField] private int _wishColorIndex;
    [SerializeField] private float _countdownTime;
    [SerializeField] private float _minWaiting, _maxWaiting;
    [SerializeField] private ColorConfig colorConfig;
    [SerializeField] private CustomerEmo emo;
    [SerializeField] private SpriteRenderer[] customerSprites;

    public void CustomerConfigure(int ColorIndex)
    {
        _wishColorIndex = ColorIndex;
    }

    private void Awake()
    {
        _minWaiting = 90f;
        _maxWaiting = 100f;
        _countdownTime = Random.Range(_minWaiting, _maxWaiting);

        emo.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // _wishColorIndex = RandomFavouriteColorIndex();
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
        emo.gameObject.SetActive(true);
        emo.ShowHappyEmo();

        LevelManager.CustomerServed?.Invoke(1);
    }

    private void Angry()
    {
        //Show angry face
        Debug.Log("I dont like this");
        //Deduce reputation
        // GameManager gameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        // if (gameManager != null)
        // {
        //     gameManager.AddReputation(10); // Add 10 to reputation
        //     Debug.Log("Added 10 reputation.");
        //}
        emo.gameObject.SetActive(true);
        emo.ShowSadEmo();
    }

    public void ShowingWishColor()
    {
        //Change ColorIndex to real color
        // Debug.Log("My favourite color is" + _wishColorIndex.ToString());

        emo.gameObject.SetActive(true);
        emo.ShowColorEmo(colorConfig.colors[_wishColorIndex]);
        //Delay;
    }

    private void Update()
    {
        //CustomerAppear
        //Waiting
        Invoke("Waiting", 1f);
        //DoneWaiting, leave
    }

    private void Leave()
    {
        Debug.Log("Left...");

        CustomerManager.Instance.RemoveCustomer(gameObject);
        Destroy(gameObject, 3f);
    }

    public void SubmitColor(int colorIndex)
    {
        ChangeColor(colorIndex);
        if (colorIndex == _wishColorIndex)
        {
            Satisfied();
            Leave();
        }
        else
        {
            Angry();
            Leave();
        }
    }

    private void ChangeColor(int colorIndex)
    {
        foreach (var spriteRenderer in customerSprites)
        {
            spriteRenderer.color = colorConfig.colors[colorIndex];
        }
    }
}