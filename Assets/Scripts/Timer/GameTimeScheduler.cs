using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeScheduler : MonoBehaviour
{
    private readonly List<(Action action, float time, string name)> _tuples = new();
    private readonly List<int> _indexesToRemove = new(256);
    private readonly object _lock = new();
    public void Schedule(Action action, float time)
    {
        var absoluteTime = Time.time + time;
        Debug.Log($"Scheduled {action.Method.Name} on {absoluteTime}");
        lock (_lock)
        {
            _tuples.Add((action, absoluteTime, name));
        }
    }
    private void Update()
    {
        _indexesToRemove.Clear();
        float currentTime = Time.time;
        for (var i = 0; i < _tuples.Count; i++)
        {
            var (action, time, name) = _tuples[i];
            if (currentTime > time)
            {
                action?.Invoke();
                _indexesToRemove.Add(i);
            }
        }
        for (var i = _indexesToRemove.Count - 1; i >= 0; i--)
        {
            _tuples.RemoveAt(_indexesToRemove[i]);
        }
    }
}