using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class Timer : MonoBehaviour
{
    public TMPro.TMP_Text textTimer;

    private Stopwatch stopwatch = new();

    void Update()
    {
        var elapsed = stopwatch.Elapsed;
        var greatThenHour = elapsed >= TimeSpan.FromHours(1);
        textTimer.text = elapsed.ToString(greatThenHour ? "hh\\:mm\\:ss" : "mm\\:ss");
    }
    public void TimeStop()
    {
        stopwatch.Stop();
    }
    public void TimeStart()
    {
        stopwatch.Start();
    }
    private void Awake()
    {
        stopwatch.Start();
    }
}
