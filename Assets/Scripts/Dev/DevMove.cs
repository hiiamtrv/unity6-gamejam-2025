using System;
using UnityEngine;

namespace Dev
{
    public class DevMove : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;

        private void Update()
        {
            transform.position += offset * Time.deltaTime;
        }
    }
}