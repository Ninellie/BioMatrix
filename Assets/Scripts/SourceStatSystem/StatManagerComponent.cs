using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StatListComponent))]
    [RequireComponent(typeof(StatSourcesComponent))]
    [AddComponentMenu("Source Stat System/Stats Manager")]
    public class StatManagerComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [field: SerializeField] public StatListComponent StatList { get; private set; }
        [field: SerializeField] public StatSourcesComponent StatSourcesList { get; private set; }

        private void Awake()
        {
            if (StatList == null) StatList = GetComponent<StatListComponent>();
            if (StatSourcesList == null) StatSourcesList = GetComponent<StatSourcesComponent>();
        }

        private void Start()
        {
            StatList.ConstructStats(StatSourcesList.GetStatSources());
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (StatList != null) StatList.ConstructStats(StatSourcesList.GetStatSources());
        }

        public void AddStatSource(StatSourceData statSourceData)
        {
            StatSourcesList.GetStatSources().Add(statSourceData);
            HandleStatChanges(statSourceData.StatId);
        }

        public void RemoveStatSource(StatSourceData statSourceData)
        {
            StatSourcesList.GetStatSources().Remove(statSourceData);
            HandleStatChanges(statSourceData.StatId);
        }

        public float GetStatValue(StatId statId)
        {
            return StatList.GetStatValue(statId);
        }

        private void HandleStatChanges(StatId statId)
        {
            // Запомнить значение стата
            var oldStatValue = StatList.GetStatValue(statId);

            // Пересчитать стат
            StatList.UpdateStat(statId, StatSourcesList.GetStatSources());

            // Получить новое значение после пересчёта
            var newStatValue = StatList.GetStatValue(statId);

            // Проверить не изменилось ли значение стата
            if (!(oldStatValue > newStatValue) && !(oldStatValue < newStatValue)) return;

            // Вызвать обсерверы
            StatOverseerSystem.Instance.NotifyObservers(gameObject.GetInstanceID(), statId, newStatValue);
        }
    }
}