using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); // on cache le panel au début
    }

    void Update()
    {
        // Touche ESC pour pause/reprendre
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // met le jeu en pause
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // reprend le jeu
        isPaused = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // reprend le temps avant de changer de scène
        SceneManager.LoadScene("MainMenu");
    }
}