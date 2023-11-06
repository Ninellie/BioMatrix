using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    public class StatOverseerSystem : MonoBehaviour
    {
        [SerializeField] private List<StatObserver> _observers = new List<StatObserver>();
        public static StatOverseerSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //public void RegistryStatEvent(UnityEvent<StatChangedEventData> onStatChangedEventData)
        //{
        //    onStatChangedEventData.AddListener(NotifyObservers);
        //}

        //public void UnRegistryStatEvent(UnityEvent<StatChangedEventData> onStatChangedEventData)
        //{
        //    onStatChangedEventData.RemoveListener(NotifyObservers);
        //}

        public void AddStatListener(StatId statId, UnityAction<float> listener, int statOwnerId)
        {
            _observers.Add(StatObserver.CreateInstance(listener, statId, statOwnerId));
        }

        public void RemoveStatListener(StatId statId, UnityAction<float> listener)
        {
            var observer = _observers.Find(o => o.action == listener && o.statId == statId);
            observer.statId = null;
            observer.action = null;
            _observers.Remove(observer);
        }

        public void NotifyObservers(StatChangedEventData statChangedEventData)
        {
            foreach (var statObserver in _observers.
                         Where(o => o.statOwnerId == statChangedEventData.entityId).
                         Where(o => o.statId == statChangedEventData.statId))
            {
                statObserver.action?.Invoke(statChangedEventData.newStatValue);
            }
        }

        public void NotifyObservers(int entityId, StatId statId, float newStatValue)
        {
            foreach (var statObserver in _observers.
                         Where(o => o.statOwnerId == entityId).
                         Where(o => o.statId == statId))
            {
                statObserver.action?.Invoke(newStatValue);
            }
        }
    }
}