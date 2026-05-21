using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;

    void Start()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CloseTutorial();
        }
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}