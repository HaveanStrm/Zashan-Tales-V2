using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [Header("Nom de la scène du Hub")]
    [SerializeField] private string hubSceneName = "Hub";
    [SerializeField] private GameObject SettingsPanel;

    // Bouton JOUER
    public void PlayGame()
    {
        SceneManager.LoadScene(hubSceneName);
    }

    // Bouton SETTINGS (pour plus tard)
    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }

    // Bouton CREDITS
    public void OpenCredits()
    {
        Debug.Log("Menu Crédits (à faire)");
    }

    // Bouton QUITTER
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu");

        Application.Quit();
    }
}