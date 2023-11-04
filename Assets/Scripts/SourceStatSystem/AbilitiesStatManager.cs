using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    //[AddComponentMenu("Source Stat System/Abilities Stat Manager")]
    //public class AbilitiesStatManager : MonoBehaviour
    //{
    //    // Один менеджер на одну способность?
    //    [field: SerializeField] public StatsHandler UnitStats { get; set; }
    //    [field: SerializeField] public List<StatsHandler> AbilityStats { get; set; }

    //    public void AddAbilityStats(StatsHandler abilityStatHandler)
    //    {
    //        if (!AbilityStats.Contains(abilityStatHandler))
    //        {
    //            AbilityStats.Add(abilityStatHandler);
    //        }
    //    }

    //    [ContextMenu("Add Ability Stat Sources")]
    //    private void AddAbilityStatSources()
    //    {
    //        foreach (var unitStatsHandler in UnitStats.StatSources)
    //        {
    //            AddStatSourceToAbilities(unitStatsHandler);
    //        }
    //    }

    //    // Эта штука подписана на стат лист героя и реагирует на его изменения. Принимая в параметре статСурс который добавляется или удаляется
    //    public void AddStatSourceToAbilities(StatSourceData statSourceData)
    //    {
    //        foreach (var abilityStatsHandler in AbilityStats)
    //        {
    //            foreach (var abilityBaseStatSource in abilityStatsHandler.BaseStatSources.StatSources)
    //            {
    //                if (abilityBaseStatSource.Id == statSourceData.Id)
    //                {
    //                    abilityStatsHandler.StatSources.Add(statSourceData);
    //                }
    //            }
    //        }
    //    }
    //}
}