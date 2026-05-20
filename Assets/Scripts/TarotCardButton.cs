using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TarotCardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite cardBack;
    [SerializeField] private Sprite cardFront;
    [SerializeField] private float flipSpeed = 0.15f;

    private Image cardImage;
    private Vector3 originalScale;
    private bool showingFront = false;
    private bool isFlipping = false;

    void Start()
    {
        cardImage = GetComponent<Image>();
        originalScale = transform.localScale;

        cardImage.sprite = cardBack;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!showingFront && !isFlipping)
        {
            StartCoroutine(FlipCard(cardFront, true));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showingFront && !isFlipping)
        {
            StartCoroutine(FlipCard(cardBack, false));
        }
    }

    IEnumerator FlipCard(Sprite newSprite, bool frontState)
    {
        isFlipping = true;

        Vector3 scale = originalScale;

        while (scale.y > 0.01f)
        {
            scale.y -= Time.unscaledDeltaTime / flipSpeed * originalScale.y;
            transform.localScale = scale;
            yield return null;
        }

        cardImage.sprite = newSprite;

        while (scale.y < originalScale.y)
        {
            scale.y += Time.unscaledDeltaTime / flipSpeed * originalScale.y;
            transform.localScale = scale;
            yield return null;
        }

        transform.localScale = originalScale;

        showingFront = frontState;
        isFlipping = false;
    }
}