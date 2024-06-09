using Core.Events;
using TMPro;
using UnityEngine;

namespace GameSession.UIScripts.BasicElements.Indicators
{
    public class GameSessionTimer : MonoBehaviour
    {
        [SerializeField] private float _winTime = 300;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private GameEvent _onVictory;

        private void Update()
        {
            var t = _winTime - Time.timeSinceLevelLoad;

            var minutes = (int)t / 60;
            var seconds = (int)t % 60;

            _textTimer.text = $"{minutes:D2}:{seconds:D2}";

            if (Time.timeSinceLevelLoad >= _winTime)
            {
                _onVictory.Raise();
            }
        }
    }
}