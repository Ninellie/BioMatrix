using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameSession.Events
{
    public class ScheduleItem
    {
        public Action Action { get; set; }
        public float AbsoluteTime { get; set; }
        public string Identifier { get; set; }
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
                var identifier = Guid.NewGuid().ToString();

                Debug.Log($"Scheduled {action.Method.Name} on {absoluteTime}");

                var item = new ScheduleItem
                {
                    Action = action,
                    AbsoluteTime = absoluteTime,
                    Identifier = identifier,
                };
                _scheduledItems.Add(item);
                return item.Identifier;
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
                    item.Action?.Invoke(); //?
                    _indexesToRemove.Add(i);
                }
            }
            for (var i = _indexesToRemove.Count - 1; i >= 0; i--)
            {
                _scheduledItems.RemoveAt(_indexesToRemove[i]);
            }
        }

        public void CallInstantly(string identifier)
        {
            GetItem(identifier).IsBeforehand = true;
        }

        public void Prolong(string identifier, float time)
        {
            GetItem(identifier).AbsoluteTime += time;
        }

        public void UpdateTime(string identifier, float time)
        {
            var item = GetItem(identifier);
            if (item.AbsoluteTime < time)
            {
                item.AbsoluteTime = time;
            }
        }

        private ScheduleItem GetItem(string name) => _scheduledItems.FirstOrDefault(scheduledItem => scheduledItem.Identifier == name);
    }
}