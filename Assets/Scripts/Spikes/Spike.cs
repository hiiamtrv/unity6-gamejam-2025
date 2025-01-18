using System;
using UnityEngine;

namespace Spikes
{
    public class Spike : MonoBehaviour
    {
        private static int bubbleLayer => LayerMask.NameToLayer("Bubble");

        private void OnTriggerEnter2D(Collider2D other)
        {
            var layer = other.gameObject.layer;
            if (layer == bubbleLayer)
            {
                Destroy(other.gameObject);
            }
        }
    }
}