using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class ProjectileMovementController : MovementController, IProjectileMovementController
    {
        [SerializeField] private float _timeToStop;

        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }
        protected override Vector2 RawMovementDirection { get; set; }

        private float SpeedDecreasePerSecond => 1 / _timeToStop;

        private new void FixedUpdate()
        {
            base.FixedUpdate();
            SlowDown(Time.fixedDeltaTime);
        }

        public void SetDirection(Vector2 direction)
        {
            RawMovementDirection = direction.normalized;
        }

        private void SlowDown(float time)
        {
            SpeedScale -= SpeedDecreasePerSecond * time;
        }
    }
}