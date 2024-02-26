using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Events
{
    [CreateAssetMenu(fileName = "New Transform GameEvent", menuName = "GameEvents/Transform", order = 51)]
    public class TransformGameEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<TransformGameEventListener> _eventListeners = new();

        public void Raise(GameObject gameObject)
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
                _eventListeners[i].OnEventRaised(gameObject);
        }

        public void RegisterListener(TransformGameEventListener listener)
        {
            if (!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void UnregisterListener(TransformGameEventListener listener)
        {
            if (_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }
    }
}