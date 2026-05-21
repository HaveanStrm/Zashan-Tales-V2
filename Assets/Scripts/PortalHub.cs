using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Scène à charger")]
    [SerializeField] private string sceneToLoad = "Level_Mat";

    [Header("UI Interaction")]
    [SerializeField] private GameObject interactText;

    private bool playerInRange = false;

    void Start()
    {
        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (interactText != null)
        {
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (interactText != null)
        {
            interactText.SetActive(false);
        }
    }
}