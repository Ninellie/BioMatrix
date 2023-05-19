using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduleItem
{
    public Action Action { get; set; }
    public float AbsoluteTime { get; set; }
    public string Name { get; set; }
    public bool IsBeforehand { get; set; }
}

public class GameTimeScheduler : MonoBehaviour
{
    private readonly List<ScheduleItem> _scheduledItems = new(256);
    private readonly List<int> _indexesToRemove = new(256);
    private readonly object _lock = new();
    
    public string Schedule(Action action, float time)
    {
        var absoluteTime = Time.time + time;
        
        lock (_lock)
        {
            var name = Guid.NewGuid().ToString();

            Debug.Log($"Scheduled {action.Method.Name} on {absoluteTime}");

            var item = new ScheduleItem
            {
                Action = action,
                AbsoluteTime = absoluteTime,
                Name = name,
            };
            _scheduledItems.Add(item);
            return item.Name;
        }
    }
    private void Update()
    {
        _indexesToRemove.Clear();
        float currentTime = Time.time;
        for (var i = 0; i < _scheduledItems.Count; i++)
        {
            var item = _scheduledItems[i];
            
            if (currentTime > item.AbsoluteTime || item.IsBeforehand)
            {
                item.Action?.Invoke();
                _indexesToRemove.Add(i);
            }
        }
        for (var i = _indexesToRemove.Count - 1; i >= 0; i--)
        {
            _scheduledItems.RemoveAt(_indexesToRemove[i]);
        }
    }

    public void CallInstantly(string name)
    {
        GetItem(name).IsBeforehand = true;
    }

    public void Prolong(string name, float time)
    {
        GetItem(name).AbsoluteTime += time;
    }

    private ScheduleItem GetItem(string name)
    {
        foreach (var scheduledItem in _scheduledItems.Where(scheduledItem => scheduledItem.Name == name))
        {
            return scheduledItem;
        }
        return null;
    }
}