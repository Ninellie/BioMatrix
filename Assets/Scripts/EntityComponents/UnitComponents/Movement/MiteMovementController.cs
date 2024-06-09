using Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class MiteMovementController : MovementController
    {
        [SerializeField] private Direction _viewDirection;
        [SerializeField] private Vector2Reference _targetPosition;
        //[SerializeField] private GameObjectReference _target;

        protected override float Speed => speed.Value * SpeedScale;

        protected override Vector2 MovementDirection
        {
            get
            {
                return _viewDirection switch
                {
                    Direction.Up => gameObject.transform.TransformVector(Vector2.up),
                    Direction.Down => gameObject.transform.TransformVector(Vector2.down),
                    Direction.Right => gameObject.transform.TransformVector(Vector2.right),
                    Direction.Left => gameObject.transform.TransformVector(Vector2.left),
                    _ => Vector2.zero
                };
            }
            set => throw new System.NotImplementedException();
        }

        //protected override Vector2 RawMovementDirection
        //{
        //    get
        //    {
        //        if (_target == null)
        //        {
        //            return Vector2.zero;
        //        }
        //        if (_target.Value.activeInHierarchy)
        //        {
        //            return (_target.Value.transform.position - transform.position).normalized;
        //        }

        //        return Vector2.zero;
        //    }
        //    set => throw new System.NotImplementedException();
        //}

        protected override Vector2 RawMovementDirection
        {
            get => (_targetPosition - (Vector2)_transform.position).normalized;
            set => throw new System.NotImplementedException();
        }
    }
}