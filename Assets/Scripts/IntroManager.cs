using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup introCanvasGroup;
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject tarotMenuPanel;
    [SerializeField] private float fadeDuration = 1f;

    private bool transitioned = false;

    void Start()
    {
        introPanel.SetActive(true);
        tarotMenuPanel.SetActive(false);
        introCanvasGroup.alpha = 1f;
    }

    void Update()
    {
        if (transitioned) return;

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            transitioned = true;
            StartCoroutine(FadeToMenu());
        }
    }

    IEnumerator FadeToMenu()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            introCanvasGroup.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }

        introCanvasGroup.alpha = 0f;
        introPanel.SetActive(false);
        tarotMenuPanel.SetActive(true);
    }
}