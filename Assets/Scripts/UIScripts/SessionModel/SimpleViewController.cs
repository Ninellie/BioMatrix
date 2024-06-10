using UnityEngine;

namespace UIScripts.SessionModel
{
    public class SimpleViewController : IViewController
    {
        private readonly ViewControllerOptions _options;

        public SimpleViewController(ViewControllerOptions options)
        {
            _options = options;
        }

        public void Freeze()
        {
            Time.timeScale = 0f;
            _options.PauseMapEvent.Raise();
        }

        public void Unfreeze()
        {
            Time.timeScale = 1f;
            _options.GameplayMapEvent.Raise();
        }

        public void Repulse()
        {
            _options.RepulseEvent.Raise();
        }

        public void OpenPauseScreen() => _options.PauseScreen.SetActive(true);
        public void ClosePauseScreen() => _options.PauseScreen.SetActive(false);
        public void CloseStartScreen() => _options.StartScreen.SetActive(false);
        public void OpenOptions() => _options.OptionsScreen.SetActive(true);
        public void CloseOptions() => _options.OptionsScreen.SetActive(false);
        public void OpenWinScreen() => _options.WinScreen.SetActive(true);
        public void OpenLoseScreen() => _options.LoseScreen.SetActive(true);

        public void CloseLevelUp()
        {
            _options.GameplayMapEvent.Raise();
            _options.LevelUpScreen.SetActive(false);
        }

        public void InitiateLevelUp()
        {
            _options.LevelUpMapEvent.Raise();
            _options.LevelUpScreen.SetActive(true);
        }
        
        public void InitiateMutation()
        {
            _options.LevelUpMapEvent.Raise();
            _options.MutationScreen.SetActive(true);
        }
        
        public void CloseMutationScreen()
        {
            _options.GameplayMapEvent.Raise();
            _options.MutationScreen.SetActive(false);
        }
    }
}