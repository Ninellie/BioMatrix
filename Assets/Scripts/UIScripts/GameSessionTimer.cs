using UnityEngine;
using System;
using TMPro;
public class GameSessionTimer : MonoBehaviour
{
    public TMP_Text textTimer;
    public Action onGameWinning;
    
    [SerializeField] private float _winTime = 300;

    private void Update()
    {
        var mins = (int)Time.time / 60;
        var secs = (int)Time.time % 60;

        textTimer.text = $"{mins:D2}:{secs:D2}";

        if (Time.time >= _winTime)
        {
            onGameWinning?.Invoke();
        }
    }
}