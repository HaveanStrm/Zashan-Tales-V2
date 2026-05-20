using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;

    void Start()
    {
        // Assure que le panel est invisible au départ
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Affiche l'écran de victoire
            if (victoryPanel != null)
                victoryPanel.SetActive(true);

            // Bloque le jeu
            Time.timeScale = 0f;
        }


    }

    public void ReturnToHub()
    {
        Time.timeScale = 1f; // Reprend le temps
        SceneManager.LoadScene("Hub"); // Nom exact de ta scène Hub
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // reprend le temps avant de changer de scène
        SceneManager.LoadScene("MainMenu");
    }
}