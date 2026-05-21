using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;

    [Header("Résolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Plein écran")]
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] resolutions;

    void Start()
    {
        SetupVolume();
        SetupResolutions();
        SetupFullscreen();
    }

    void Update()
    {

    }

    void SetupVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        AudioListener.volume = savedVolume;
    }

    void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void SetupFullscreen()
    {
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutions.Length == 0) return;

        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(
            resolution.width,
            resolution.height,
            Screen.fullScreen
        );
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}