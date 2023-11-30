using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    [CreateAssetMenu(fileName = "New Player Target Set", menuName = "Sets/Player target", order = 51)]
    public class PlayerTargetRuntimeSet : RuntimeSet<PlayerTarget>
    { }
}