using GameManager;
using Level;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [Header("Values")] [SerializeField] private int _wishColorIndex;
    [SerializeField] private float _countdownTime;
    [SerializeField] private float _minWaiting, _maxWaiting;
    
    public void CustomerConfigure(int ColorIndex)
    {
        _wishColorIndex = ColorIndex;
    }

    private void Awake()
    {
        _minWaiting = 90f;
        _maxWaiting = 100f;
        _countdownTime = Random.Range(_minWaiting, _maxWaiting);
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

    private void Leave()
    {
        Debug.Log("Left...");
        
        CustomerManager.Instance.RemoveCustomer(gameObject);
        // Destroy(gameObject, 1f);
    }

    public void SubmitColor(int colorIndex)
    {
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
}