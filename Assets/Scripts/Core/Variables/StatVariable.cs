using System.Collections.Generic;
using Core.Events;
using SourceStatSystem;
using UnityEngine;

namespace Core.Variables
{
    [CreateAssetMenu(fileName = "New Stat Variable", menuName = "Variables/Stat", order = 51)]
    public class StatVariable : FloatVariable
    {
        [SerializeField] private List<StatId> idList;
        
        public List<StatId> IdList => idList;
        
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<StatListener> _eventListeners = new();

        public static implicit operator float(StatVariable reference)
        {
            return reference.value;
        }

        public void Raise()
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
                _eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(StatListener listener)
        {
            if (!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void UnregisterListener(StatListener listener)
        {
            if (_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }

        public new void SetValue(float value)
        {
            var oldValue = this.value;
            this.value = value;
            TryRaiseEvent(oldValue);
        }

        public new void SetValue(FloatVariable value)
        {
            var oldValue = this.value;
            this.value = value.value;
            TryRaiseEvent(oldValue);
        }

        public void SetValue(StatVariable value)
        {
            var oldValue = this.value;
            this.value = value.value;
            TryRaiseEvent(oldValue);
        }

        public new void ApplyChange(float amount)
        {
            var oldValue = value;
            value += amount;
            TryRaiseEvent(oldValue);
        }

        public new void ApplyChange(FloatVariable amount)
        {
            var oldValue = value;
            value += amount.value;
            TryRaiseEvent(oldValue);
        }

        public void ApplyChange(StatVariable amount)
        {
            var oldValue = value;
            value += amount.value;
            TryRaiseEvent(oldValue);
        }

        private void TryRaiseEvent(float oldValue)
        {
            if (oldValue.Equals(value)) return;
            Raise();
        }
    }
}