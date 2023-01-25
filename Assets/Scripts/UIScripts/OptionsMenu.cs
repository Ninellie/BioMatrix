using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Action onBackToMainMenu;

    private Resolution[] _resolutions;

    private void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = _resolutions.Select(x => x.width + " x " + x.height).ToList();
        var c = Screen.currentResolution;
        var i = Array.FindIndex(_resolutions, x => x.width == c.width && x.height == c.height);
        var currentResolutionIndex = i < 0 ? 0 : i;
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void BackToMainMenu()
    {
        onBackToMainMenu?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}