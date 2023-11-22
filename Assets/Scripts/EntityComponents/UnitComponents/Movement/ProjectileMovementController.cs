using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class ProjectileMovementController : MovementController
    {
        [SerializeField] private UnityEvent _onStopped;
        private bool _onStoppedEventSended;
        protected override float Speed => speed.Value * SpeedScale;
        protected override Vector2 MovementDirection
        {
            get => RawMovementDirection;
            set => throw new System.NotImplementedException();
        }
        protected override Vector2 RawMovementDirection { get; set; }

        private new void FixedUpdate()
        {
            base.FixedUpdate();
            if (_onStoppedEventSended) return;
            if (!IsStopped()) return;
            _onStopped.Invoke();
        }

        public void SetDirection(Vector2 direction) => RawMovementDirection = direction.normalized;
    }
}