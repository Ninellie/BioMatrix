using UnityEngine;
using System;
using System.Diagnostics;
using TMPro;
public class GameTimer : MonoBehaviour
{
    public TMP_Text textTimer;
    public Action onGameWinning;
    private const float WinTime = 600;
    private Stopwatch _stopwatch;

    private void Awake()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    private void Update()
    {
        TimerUIUpdate();
        if (_stopwatch.Elapsed.TotalSeconds > WinTime)
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
        return GetTotalSeconds() > WinTime;
    }
}