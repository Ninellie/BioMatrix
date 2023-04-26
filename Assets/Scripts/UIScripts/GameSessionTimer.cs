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

        textTimer.text = mins switch
        {
            < 60 => secs < 10 ? $"{mins}:0{secs}" : $"{mins}:{secs}",
            _ => mins switch
            {
                < 10 => secs < 10 ? $"0{mins}:0{secs}" : $"0{mins}:{secs}",
                _ => mins switch
                {
                    < 1 => secs < 10 ? $"00:0{secs}" : $"00:{secs}",
                    _ => textTimer.text
                }
            }
        };

        if (Time.time >= _winTime)
        {
            onGameWinning?.Invoke();
        }
    }
}