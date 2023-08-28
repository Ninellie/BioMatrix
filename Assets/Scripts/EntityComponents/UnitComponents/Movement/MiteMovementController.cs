using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class MiteMovementController : MovementController, ITargeted
    {
        [SerializeField]
        private Direction _viewDirection;

        [SerializeField]
        private GameObject _target;

        protected override float Speed => speedStat.Value * SpeedScale;

        protected override Vector2 MovementDirection
        {
            get
            {
                return _viewDirection switch
                {
                    Direction.Up => gameObject.transform.TransformVector(0, 1, 0),
                    Direction.Down => gameObject.transform.TransformVector(0, -1, 0),
                    Direction.Right => gameObject.transform.TransformVector(1, 0, 0),
                    Direction.Left => gameObject.transform.TransformVector(-1, 0, 0),
                    _ => Vector2.zero
                };
            }
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

                return Vector2.zero;
            }
            set => throw new System.NotImplementedException();
        }

        public void SetTarget(GameObject target)
        {
            _target = target;
        }
    }
}