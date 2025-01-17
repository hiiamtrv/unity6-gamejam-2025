using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dev
{
    public class DevMove : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool randomMove;

        private void Start()
        {
            if (randomMove)
            {
                offset = new Vector3(
                    Random.Range(-5f, 5f),
                    1f,
                    0f
                );
            }
        }

        private void Update()
        {
            transform.position += offset * Time.deltaTime;
        }
    }
}