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
            Debug.Log($"Check {other.gameObject.name} {layer} {bubbleLayer}");
            if (layer == bubbleLayer)
            {
                Destroy(other.gameObject);
            }
        }
    }
}