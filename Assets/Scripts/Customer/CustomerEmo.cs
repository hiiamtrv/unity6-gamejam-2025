using System;
using DG.Tweening;
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


    private float originScale;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        originScale = transform.localScale.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ShowColorEmo(Color color)
    {
        spriteRenderer.sprite = colorEmo;

        colorWishEmo.gameObject.SetActive(true);
        colorWishEmo.color = color;
        AnimEmo(false);
    }

    public void ShowHappyEmo()
    {
        var randIdx = Random.Range(0, happyEmo.Length);
        spriteRenderer.sprite = happyEmo[randIdx];
        AnimEmo(true);

        colorWishEmo.gameObject.SetActive(false);
    }

    public void ShowSadEmo()
    {
        var randIdx = Random.Range(0, sadEmo.Length);
        spriteRenderer.sprite = sadEmo[randIdx];
        AnimEmo(true);

        colorWishEmo.gameObject.SetActive(false);
    }

    private void AnimEmo(bool emphasize)
    {
        var nextEase = Ease.OutCirc;
        if (emphasize)
        {
            var eases = new[] { Ease.OutBounce, Ease.OutBack, Ease.OutCirc };
            nextEase = eases[Random.Range(0, eases.Length)];
        }

        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, 1.2f).SetEase(nextEase);
    }
}