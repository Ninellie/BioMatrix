using UnityEngine;
using System;
using System.Diagnostics;
using TMPro;
public class GameSessionTimer : MonoBehaviour
{
    public TMP_Text textTimer;
    public Action onGameWinning;
    private const float WinTime = 300;
    private Stopwatch _stopwatch;

    private void Awake()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    private void Update()
    {
        TimerUIUpdate();
        if (IsTimeToWin())
        {
            onGameWinning?.Invoke();
        }
    }
    public void Stop() { _stopwatch.Stop(); }
    public void Resume() { _stopwatch.Start(); }
    public float GetTotalSeconds()
    {
        var ts = _stopwatch.Elapsed;
        return (float)ts.TotalSeconds;
    }
    private void TimerUIUpdate()
    {
        var elapsed = _stopwatch.Elapsed;
        var greatThenHour = elapsed >= TimeSpan.FromHours(1);
        textTimer.text = elapsed.ToString(greatThenHour ? "hh\\:mm\\:ss" : "mm\\:ss");
    }
    private bool IsTimeToWin()
    {
        return _stopwatch.Elapsed.TotalSeconds > WinTime;
    }
}