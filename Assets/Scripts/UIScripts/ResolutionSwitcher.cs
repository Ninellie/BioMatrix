using System;
using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class ResolutionSwitcher : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resolutionText;
        [SerializeField] private Resolution[] _resolutions;

        private int _currentIndex;

        private void Start()
        {
            _resolutions = Screen.resolutions;
            var currentResolution = Screen.currentResolution;
            _currentIndex = Array.IndexOf(_resolutions, currentResolution);
            _resolutionText.SetText(currentResolution.ToString());
        }

        public void SwitchToNextResolution()
        {
            var nextIndex = _currentIndex + 1;
            nextIndex = nextIndex > _resolutions.Length - 1 ? 0 : nextIndex;
            var nextResolution = _resolutions[nextIndex];
            _resolutionText.SetText(nextResolution.ToString());
            Screen.SetResolution(nextResolution.width, nextResolution.height, Screen.fullScreen);
            _currentIndex = nextIndex;
        }
    }
}