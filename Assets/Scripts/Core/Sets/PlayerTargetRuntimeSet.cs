using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    [CreateAssetMenu(fileName = "New Player Target Set", menuName = "Sets/Player target", order = 51)]
    public class PlayerTargetRuntimeSet : RuntimeSet<PlayerTarget>
    {
        public PlayerTarget GetNearestToCenterInCircle(Vector2 center, float radius)
        {
            var distanceToNearestTarget = Mathf.Infinity;
            PlayerTarget nearestTarget = null;
            foreach (var target in items)
            {
                var distance = Vector2.Distance(center, target.transform.position);
                if (!(distance < radius)) continue;
                if (!(distance < distanceToNearestTarget)) continue;
                distanceToNearestTarget = distance;
                nearestTarget = target;
            }
            return nearestTarget;
        }

        public PlayerTarget GetNearestToPosition(Vector2 position)
        {
            return GetNearestToCenterInCircle(position, Mathf.Infinity);
        }
    }
}