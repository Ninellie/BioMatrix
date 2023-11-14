using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class DistanceShooterEnemy : MonoBehaviour
    {
        [SerializeField] private GameObjectReference _target;
        [SerializeField] private float _distance;
        [SerializeField] private BombardierFirearm _firearm;

        private void FixedUpdate()
        {
            TryUseAbility();
        }

        private void TryUseAbility()
        {
            var currentDistance = Vector2.Distance(transform.position, _target.Value.transform.position);
            if (currentDistance < _distance)
            {
                _firearm.TryDoAction();
            }
        }
    }
}