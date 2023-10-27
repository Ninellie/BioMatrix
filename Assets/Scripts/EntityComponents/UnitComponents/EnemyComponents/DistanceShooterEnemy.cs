using Assets.Scripts.FirearmComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class DistanceShooterEnemy : MonoBehaviour, ITargeted
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _distance;
        [SerializeField] private BombardierFirearm _firearm;

        private void FixedUpdate()
        {
            TryUseAbility();
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }

        private void TryUseAbility()
        {
            var currentDistance = Vector2.Distance(transform.position, _target.transform.position);
            if (currentDistance < _distance)
            {
                _firearm.TryDoAction();
            }
        }
    }
}