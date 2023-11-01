using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Source Stat System/Stats Handler")]
    public class StatsHandler : MonoBehaviour, ISerializationCallbackReceiver
    {
        // Отдельный хендлер для каждого стата

        // Или отдельный хендлер для каждого ТИПА стата. Стат может иметь свои зависимости и может зависеть от определённых событий 
        // К временных эффектам добавить повышение стата если за последнее время персонаж делал что-то. 
        // Эта логика может выполняться как эффектом: эффект отслеживает события и добавляет-убавляет стат-сурс (модификатор), как оно есть сейчас
        // Так и заведением нового стата, например "+2% скорости эффекта если за последние 4 секунды было совершено убийство" - стат хендлер уже подписан на нужные эффекты и знает сколько было совершено убийств за последние 5 секунд и вернёт нужное значение (не лучшая идея, но интересная)

        [field: SerializeField] public BaseStatSources BaseStatSources { get; private set; }
        [field: SerializeField] public List<StatSourceData> StatSources { get; private set; } = new List<StatSourceData>();
        [field: SerializeField] public List<StatData> Stats { get; private set; } = new List<StatData>();

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
        void ISerializationCallbackReceiver.OnAfterDeserialize() => ConstructStats();

        private void ConstructStats()
        {
            Stats = StatSources.Where(baseStatSource => baseStatSource.StatId != null).
                GroupBy(statSource => statSource.StatId).
                Select(group => new StatData
                {
                    Id = group.Key.Id,
                    Value = group.Where(stat => stat.SourceImpactType == ImpactType.Flat).Sum(stat => stat.Value)
                            * (1 + group.Where(stat => stat.SourceImpactType == ImpactType.Percentage).Sum(stat => stat.Value) * 0.01f),
                }).ToList();

            foreach (var statData in Stats)
            {
                statData.inspectorValue = $"{statData.Value} - {statData.Id}";
            }
        }
    }
}