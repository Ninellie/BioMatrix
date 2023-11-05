using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    public class StatSubjectComponent : MonoBehaviour
    {
        [SerializeField] private List<StatObserver> _observers = new List<StatObserver>();

        public void CallObservers(StatId statId, float statValue)
        {
            foreach (var statObserver in _observers.Where(o => o.statId == statId))
            {
                statObserver.action?.Invoke(statValue);
            }
        }

        public void AddStatListener(StatId statId, UnityAction<float> listener)
        {
            _observers.Add(StatObserver.CreateInstance(listener, statId));
        }

        public void RemoveStatListener(StatId statId, UnityAction<float> listener)
        {
            var observer = _observers.Find(o => o.action == listener && o.statId == statId);
            _observers.Remove(observer);
        }
    }
}