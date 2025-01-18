using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;
    public Transform[] SpawnPoints;
    private bool[] OccupiedPoints;
    [SerializeField] private float destroyCustomerTime = 2f;
    [SerializeField] private float waitForDestructionTime = 3f;
    private void Start()
    {
        OccupiedPoints = new bool[SpawnPoints.Length];
        SpawnedAllPoints();
    }

    private void SpawnedAllPoints()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (!OccupiedPoints[i])
            {
                SpawnObjectAtPoint(SpawnPoints[i],i);
            }
        }
    }

    private void SpawnObjectAtPoint(Transform point, int pointIndex)
    {
        Customer customer = Instantiate(customerPrefab, point.position, point.rotation);
        customer.AddIndex(pointIndex);
        OccupiedPoints[pointIndex] = true;
        Destroy(customer.gameObject, destroyCustomerTime);
        StartCoroutine(WaitForDestruction(customer, pointIndex));
        
    }

    private IEnumerator WaitForDestruction(Customer customer, int pointIndex)
    {
        var elapsedTime = 0f;
        while (elapsedTime < waitForDestructionTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("end delay");
        if (customer != null)
        {
            Destroy(customer.gameObject);
        }
        OccupiedPoints[pointIndex] = false;
        RefillSpawnPoint(pointIndex);
    }

    internal void RefillSpawnPoint(int pointIndex)
    {
        Debug.Log("refill");
        if (!OccupiedPoints[pointIndex])
        {
            Debug.Log("spawn next");
            SpawnObjectAtPoint(SpawnPoints[pointIndex],pointIndex);
        }
    }

}
