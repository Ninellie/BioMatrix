using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class DistanceKeeperMovementController : MovementController, ITargeted
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _distance;

        protected override float Speed
        {
            get
            {
                if (_target == null) return speedStat.Value * SpeedScale;

                var currentDistance = Vector2.Distance(transform.position, _target.transform.position);
                
                if (currentDistance < _distance)
                {
                    return 0f;
                }
                return speedStat.Value * SpeedScale;
            }
        }

        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }

        protected override Vector2 RawMovementDirection
        {
            get
            {
                if (_target == null)
                {
                    return Vector2.zero;
                }
                if (_target.activeInHierarchy)
                {
                    return (_target.transform.position - transform.position).normalized;
                }
                else
                {
                    return Vector2.zero;
                }
            }
            set => throw new System.NotImplementedException();
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
    }
}