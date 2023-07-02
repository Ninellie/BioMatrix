using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
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
            _player = FindObjectOfType<Player>();
            _player.Lvl.IncrementEvent += UpdateLevelBar;
            _player.Exp.ValueChangedEvent += UpdateExperienceBar;
            _player.KillsCount.ValueChangedEvent += UpdateKillsBar;
            _player.OnDeath += Unsubscription;
        }
        
        private void Unsubscription()
        {
            _player.Lvl.IncrementEvent -= UpdateLevelBar;
            _player.Exp.ValueChangedEvent -= UpdateExperienceBar;
            _player.KillsCount.ValueChangedEvent -= UpdateKillsBar;
            _player.OnDeath -= Unsubscription;
        }

        private void UpdateExperienceBar()
        {
            var expToNextLvlValue = _player.Exp.GetLackValue();
            var experienceToNextLevelText = $"exp to next level: {expToNextLvlValue}";
            experienceToNextLevelBar.text = experienceToNextLevelText;
            Debug.Log("Experience bar was updated");
        }

        private void UpdateLevelBar()
        {
            var levelText = $"LVL: {_player.Lvl.GetValue()}";
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