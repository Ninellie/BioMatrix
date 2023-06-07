using Assets.Scripts.Entity.Unit.Player;
using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.View
{
    public class ExpUI : MonoBehaviour
    {
        public TMPro.TMP_Text experienceToNextLevelBar;
        public TMPro.TMP_Text levelBar;
        public TMPro.TMP_Text killsBar;
        private Player _player;
        private void Start()
        {
            UpdateExperienceBar();
            UpdateLevelBar();
        }
        
        public void Subscription()
        {
            Debug.Log("Experience and level started subscribing on current exp and level of Player");
            _player = FindObjectOfType<Player>();
            _player.LevelUpEvent += UpdateLevelBar;
            _player.ExperienceTakenEvent += UpdateExperienceBar;
            _player.KillsCount.ValueChangedEvent += UpdateKillsBar;
            _player.OnDeath += Unsubscription;
        }
        
        private void Unsubscription()
        {
            Debug.Log("Experience and level started unsubscribing on current exp and level of Player");
            _player.LevelUpEvent -= UpdateLevelBar;
            _player.ExperienceTakenEvent -= UpdateExperienceBar;
            _player.KillsCount.ValueChangedEvent -= UpdateKillsBar;
            _player.OnDeath -= Unsubscription;
        }

        private void UpdateExperienceBar()
        {
            var experienceToNextLevelText = $"exp to lvl up: {_player.ExpToLvlup}";

            experienceToNextLevelBar.text = experienceToNextLevelText;
            Debug.Log("Experience bar was updated");
        }

        private void UpdateLevelBar()
        {
            var levelText = $"LVL:  + {_player.Level}";
            levelBar.text = levelText;
            Debug.Log("Level bar was updated");
        }

        private void UpdateKillsBar()
        {
            var killsText = $"Kills: {_player.KillsCount.GetValue()}";
            killsBar.text = killsText;
        }
    }
}