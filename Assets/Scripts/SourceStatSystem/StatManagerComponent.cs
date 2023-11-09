using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StatListComponent))]
    [RequireComponent(typeof(StatSourcesComponent))]
    [AddComponentMenu("Source Stat System/Stats Manager")]
    public class StatManagerComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private StatListComponent _statList;
        [SerializeField] private StatSourcesComponent _statSourcesList;

        private void Awake()
        {
            if (_statList == null) _statList = GetComponent<StatListComponent>();
            if (_statSourcesList == null) _statSourcesList = GetComponent<StatSourcesComponent>();
        }

        private void Start()
        {
            _statList.ConstructStats(_statSourcesList.GetStatSources());
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_statList != null) _statList.ConstructStats(_statSourcesList.GetStatSources());
        }

        public void AddStatSource(StatSourceData statSourceData)
        {
            _statSourcesList.GetStatSources().Add(statSourceData);
            HandleStatChanges(statSourceData.StatId);
        }

        public void RemoveStatSource(StatSourceData statSourceData)
        {
            _statSourcesList.GetStatSources().Remove(statSourceData);
            HandleStatChanges(statSourceData.StatId);
        }

        public float GetStatValue(StatId statId)
        {
            return _statList.GetStatValue(statId);
        }

        private void HandleStatChanges(StatId statId)
        {
            // Запомнить значение стата
            var oldStatValue = _statList.GetStatValue(statId);

            // Пересчитать стат
            _statList.UpdateStat(statId, _statSourcesList.GetStatSources());

            // Получить новое значение после пересчёта
            var newStatValue = _statList.GetStatValue(statId);

            // Проверить не изменилось ли значение стата
            if (!(oldStatValue > newStatValue) && !(oldStatValue < newStatValue)) return;

            // Вызвать обсерверы
            StatOverseerSystem.Instance.NotifyObservers(gameObject.GetInstanceID(), statId, newStatValue);
        }
    }
}