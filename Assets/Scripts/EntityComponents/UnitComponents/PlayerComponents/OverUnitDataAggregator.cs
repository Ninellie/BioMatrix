using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [AddComponentMenu("Entity/OverUnitDataAggregator")]
    public class OverUnitDataAggregator : MonoBehaviour
    {
        public Dictionary<TargetName, StatList> Stats { get; } = new();
        public Dictionary<TargetName, ResourceList> Resources { get; } = new();
        public Dictionary<TargetName, EffectsList> Effects { get; } = new();

        public void ReadInfoFromTarget(GameObject target, TargetName targetName)
        {
            Stats.Add(targetName, target.GetComponent<StatList>());
            Resources.Add(targetName, target.GetComponent<ResourceList>());
            Effects.Add(targetName, target.GetComponent<EffectsList>());
        }

        public void AddEffect(IEffect effect)
        {
            Effects[effect.TargetName].AddEffect(effect);
        }
    }
}