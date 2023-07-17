using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.GameSession.UIScripts.View
{
    public class GameSessionTimer : MonoBehaviour
    {
        public TMP_Text textTimer;
        public Action onGameWinning;
    
        [SerializeField] private float _winTime = 300;

        private void Update()
        {
            var mins = (int)Time.timeSinceLevelLoad / 60;
            var secs = (int)Time.timeSinceLevelLoad % 60;

            textTimer.text = $"{mins:D2}:{secs:D2}";

            if (Time.timeSinceLevelLoad >= _winTime)
            {
                onGameWinning?.Invoke();
            }
        }
    }
}