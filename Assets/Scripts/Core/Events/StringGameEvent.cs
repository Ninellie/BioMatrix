using System.Collections.Generic;
using UnityEngine;

namespace Core.Events
{
    [CreateAssetMenu(fileName = "New String Event", menuName = "GameEvents/String", order = 51)]
    public class StringGameEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<StringGameEventListener> _eventListeners = new();

        public void Raise(string str)
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
                _eventListeners[i].OnEventRaised(str);
        }

        public void RegisterListener(StringGameEventListener listener)
        {
            if (!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void UnregisterListener(StringGameEventListener listener)
        {
            if (_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }
    }
}