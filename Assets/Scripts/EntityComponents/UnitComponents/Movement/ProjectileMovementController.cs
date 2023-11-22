using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class ProjectileMovementController : MovementController
    {
        protected override float Speed => speedStat.Value * SpeedScale;
        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }
        protected override Vector2 RawMovementDirection { get; set; }

        private new void FixedUpdate() => base.FixedUpdate();

        public void SetDirection(Vector2 direction) => RawMovementDirection = direction.normalized;
    }
}