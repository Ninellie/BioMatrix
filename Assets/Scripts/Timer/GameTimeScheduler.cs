using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTimeScheduler : MonoBehaviour
{
    private readonly List<(Action action, float time, string name)> _tuples = new();
    private readonly List<int> _indexesToRemove = new(256);
    private readonly object _lock = new();
    public void Schedule(Action action, float time)
    {
        var absoluteTime = Time.time + time;
        Debug.Log($"Scheduled {action} on {absoluteTime}");
        lock (_lock)
        {
            _tuples.Add((action, absoluteTime, name));
        }
    }

    public void Overwrite(float time, string name)
    {
        List<(Action action, float time, string name)> tuplesToRemove = new();
        List<(Action action, float time, string name)> tuplesToAdd = new();
        foreach (var tuple in _tuples.Where(x => x.name == name))
        {
            var overwrittenTuple = tuple;
            overwrittenTuple.time = time;
            tuplesToRemove.Add(tuple);
            tuplesToAdd.Add(overwrittenTuple);
        }

        foreach (var tuple in tuplesToRemove)
        {
            _tuples.Remove(tuple);
        }
        foreach (var tuple in tuplesToAdd)
        {
            _tuples.Add(tuple);
        }
    }
    public void Extend(float time, string name)
    {
        List<(Action action, float time, string name)> tuplesToRemove = new();
        List<(Action action, float time, string name)> tuplesToAdd = new();
        foreach (var tuple in _tuples.Where(x => x.name == name))
        {
            var overwritedTuple = tuple;
            overwritedTuple.time = tuple.time + time;
            tuplesToRemove.Add(tuple);
            tuplesToAdd.Add(overwritedTuple);
        }

        foreach (var tuple in tuplesToRemove)
        {
            _tuples.Remove(tuple);
        }
        foreach (var tuple in tuplesToAdd)
        {
            _tuples.Add(tuple);
        }
    }

    public float[] GetTimes(string actionName)
    {
        // Получает массив назначенных времён у всех отложенных событий по имени
        List<float> times = new();
        foreach (var tuple in _tuples.Where(x => x.name == actionName))
        {
            times.Add(tuple.time);
        }
        return times.ToArray();
    }
    public float[] GetRemainingTimes(string actionName)
    {
        // Получает массив времени до всех отложенных событий по имени
        return _tuples.Where(x => x.name == actionName).Select(tuple => tuple.time - Time.time).ToArray();
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