using UnityEngine;

public class BackgroundChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public void ChangeBackground(Sprite newBackground)
    {
        if (newBackground == null) return;

        spriteRenderer.sprite = newBackground;
    }
}