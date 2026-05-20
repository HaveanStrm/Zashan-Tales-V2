using System.Collections;
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Coroutine hideCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    void Update()
    {

    }

    public void Reveal(float duration)
    {
        if (spriteRenderer == null) return;

        spriteRenderer.enabled = true;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterDelay(duration));
    }

    private IEnumerator HideAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);

        spriteRenderer.enabled = false;
    }
}