using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.View
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        
        public Action onBackToMainMenu;

        



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
}