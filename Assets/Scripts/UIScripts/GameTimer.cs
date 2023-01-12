using UnityEngine;
using System;
using System.Diagnostics;

public class GameTimer : MonoBehaviour
{
    public TMPro.TMP_Text textTimer;
    private readonly Stopwatch _stopwatch = new();
    private void Awake()
    {
        _stopwatch.Start();
    }
    private void Update()
    {
        var elapsed = _stopwatch.Elapsed;
        var greatThenHour = elapsed >= TimeSpan.FromHours(1);
        textTimer.text = elapsed.ToString(greatThenHour ? "hh\\:mm\\:ss" : "mm\\:ss");
    }
    public void TimeStop()
    {
        _stopwatch.Stop();
    }
    public void TimeStart()
    {
        _stopwatch.Start();
    }
    public float GetTotalSeconds()
    {
        var ts = _stopwatch.Elapsed;
        return (float)ts.TotalSeconds;
    }
}
