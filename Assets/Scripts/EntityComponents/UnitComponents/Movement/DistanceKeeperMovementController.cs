using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class DistanceKeeperMovementController : MovementController
    {
        [SerializeField] private GameObjectReference _target;
        [SerializeField] private float _distance;

        protected override float Speed
        {
            get
            {
                if (_target == null) return speed.Value * SpeedScale;

                var currentDistance = Vector2.Distance(transform.position, _target.Value.transform.position);
                
                if (currentDistance < _distance)
                {
                    return 0f;
                }
                return speed.Value * SpeedScale;
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
                if (_target.Value.activeInHierarchy)
                {
                    return (_target.Value.transform.position - transform.position).normalized;
                }
                else
                {
                    return Vector2.zero;
                }
            }
            set => throw new System.NotImplementedException();
        }
    }
}