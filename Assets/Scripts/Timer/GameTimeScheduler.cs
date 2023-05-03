using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeScheduler : MonoBehaviour
{
    private readonly List<(Action action, float time)> _tuples = new();
    private readonly List<int> _indexesToRemove = new(256);
    private readonly object _lock = new();
    public void Schedule(Action action, float time)
    {
        lock (_lock)
        {
            _tuples.Add((action, time));
        }
    }
    private void Update()
    {
        _indexesToRemove.Clear();
        float currentTime = Time.time;
        for (var i = 0; i < _tuples.Count; i++)
        {
            var (action, time) = _tuples[i];
            if (currentTime > time)
            {
                action?.Invoke();
                _indexesToRemove.Add(i);
            }
        }

        foreach (var i in _indexesToRemove)
        {
            _tuples.RemoveAt(i);
        }
    }
}