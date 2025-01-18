using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CustomerEmo : MonoBehaviour
{
    [FormerlySerializedAs("colorEmo")] [SerializeField]
    private SpriteRenderer colorWishEmo;

    [SerializeField] private Sprite colorEmo;
    [SerializeField] private Sprite[] happyEmo;
    [SerializeField] private Sprite[] sadEmo;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowColorEmo(Color color)
    {
        spriteRenderer.sprite = colorEmo;

        colorWishEmo.gameObject.SetActive(true);
        colorWishEmo.color = color;
    }

    public void ShowHappyEmo()
    {
        var randIdx = Random.Range(0, happyEmo.Length);
        spriteRenderer.sprite = happyEmo[randIdx];

        colorWishEmo.gameObject.SetActive(false);
    }

    public void ShowSadEmo()
    {
        var randIdx = Random.Range(0, sadEmo.Length);
        spriteRenderer.sprite = sadEmo[randIdx];

        colorWishEmo.gameObject.SetActive(false);
    }
}