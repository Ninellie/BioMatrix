using UnityEngine;
using System;
using System.Diagnostics;

public class GameTimer : MonoBehaviour
{
    public TMPro.TMP_Text textTimer;

    private Stopwatch stopwatch = new();
    private void Update()
    {
        var elapsed = stopwatch.Elapsed;
        var greatThenHour = elapsed >= TimeSpan.FromHours(1);
        textTimer.text = elapsed.ToString(greatThenHour ? "hh\\:mm\\:ss" : "mm\\:ss");
    }
    private void Awake()
    {
        stopwatch.Start();
    }
    public void TimeStop()
    {
        stopwatch.Stop();
    }
    public void TimeStart()
    {
        stopwatch.Start();
    }
    public float GetTotalSeconds()
    {
        var ts = stopwatch.Elapsed;
        return (float)ts.TotalSeconds;
    }
}
