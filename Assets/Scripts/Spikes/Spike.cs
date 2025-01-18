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
                SFXManager.Instance.PlayPopSound();
                Destroy(other.gameObject);
            }
        }
    }
}