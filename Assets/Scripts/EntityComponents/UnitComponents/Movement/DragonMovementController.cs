using Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class DragonMovementController : MovementController
    {
        [SerializeField] private Vector2Reference _targetPosition;
        //[SerializeField] private GameObjectReference _target;

        protected override float Speed => speed.Value * SpeedScale;

        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }

        protected override Vector2 RawMovementDirection
        {
            get => (_targetPosition - (Vector2)_transform.position).normalized;
            set => throw new System.NotImplementedException();
        }
    }
}