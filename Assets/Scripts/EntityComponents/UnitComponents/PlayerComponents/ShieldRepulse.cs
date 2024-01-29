using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class ShieldRepulse : MonoBehaviour
    {
        [SerializeField] private LayerMask _resistancePhysLayer;
        [SerializeField] private FloatReference _radius;
        [SerializeField] private FloatReference _force;

        public void Repulse()
        {
            var nearbyEnemies = GetNearbyEnemiesKnockbackControllerList(_radius, _resistancePhysLayer);
            foreach (var knockbackController in nearbyEnemies)
            {
                if (knockbackController == null)
                {
                    continue;
                }
                var force = (Vector2)knockbackController.transform.position - (Vector2)gameObject.transform.position;
                force.Normalize();
                force *= _force;
                knockbackController.Knockback(force);
            }
        }

        private List<KnockbackController> GetNearbyEnemiesKnockbackControllerList(float repulseRadius, LayerMask enemyLayer)
        {
            var colliders2D = Physics2D.OverlapCircleAll(transform.position, repulseRadius, enemyLayer);
            return colliders2D.Select(collider2d => collider2d.gameObject.GetComponent<KnockbackController>()).ToList();
        }
    }
}