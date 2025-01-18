using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Customer : MonoBehaviour
{
    private CustomerManager manager;
    [Header("Values")]
    [SerializeField] private int _wishColorIndex;
    [SerializeField] private float _timer;
    [SerializeField] private int _spawnIndex;

    public void CustomerConfigure( int ColorIndex)
    {
        _wishColorIndex = ColorIndex;
    }
    
    private void Awake()
    {
        manager = FindFirstObjectByType<CustomerManager>();
    }
    public void AddIndex(int Index)
    {
        _spawnIndex=Index;
    }
    private void Update()
    {

    }
}
