using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite startingBackground;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingBackground = spriteRenderer.sprite;
    }

    public void ChangeBackground(Sprite newBackground)
    {
        if (newBackground == null) return;

        spriteRenderer.sprite = newBackground;
    }

    public void ResetBackground()
    {
        spriteRenderer.sprite = startingBackground;
    }
}