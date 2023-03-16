using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer
{
    private Action _action;
    private float _time;
    private bool _isDestroyed;

    public GameTimer(Action action, float time)
    {
        _action = action;
        _time = time;
        _isDestroyed = false;
    }

    public void Update()
    {
        if (_isDestroyed) return;
        _time -= Time.deltaTime;
        if (!(_time < 0)) return;
        //Trigger the action
        _action();
        DestroySelf();
    }

    private void DestroySelf()
    {
        _isDestroyed = true;
    }
}
