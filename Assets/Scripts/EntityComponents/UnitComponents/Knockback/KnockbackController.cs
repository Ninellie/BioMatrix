using System.Collections;
using System.Numerics;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.EntityComponents.UnitComponents.Knockback
{
    [RequireComponent(typeof(IMovementController))]
    public class KnockbackController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("In points per seconds")]
        private float _knockbackSpeed;

        private IMovementController _movementController;
        private Vector2 MyPosition => transform.position;
        private Vector2 _currentAddedVelocity = Vector2.zero;

        protected void Awake()
        {
            _movementController = GetComponent<IMovementController>();
        }

        public void Knockback(Vector2 force)
        {
            var knockbackTime = force.magnitude / _knockbackSpeed;
            var knockbackDirection = force.normalized;
            var knockbackVelocity = knockbackDirection * _knockbackSpeed;
            if (!_currentAddedVelocity.Equals(Vector2.zero))
            {
                CancelInvoke(nameof(RemoveVelocity));
                RemoveVelocity();
            }
            AddVelocity(knockbackVelocity);
            Invoke(nameof(RemoveVelocity), knockbackTime);
        }

        public void Knockback(GameObject target)
        {
            var force = MyPosition - (Vector2)target.transform.position;
            force.Normalize();
            force *= target.GetComponent<Entity>().KnockbackPower.Value;
            Knockback(force);
        }

        private void AddVelocity(Vector2 velocity)
        {
            _movementController.AddVelocity(velocity);
            _currentAddedVelocity = velocity;
        }

        private void RemoveVelocity()
        {
            _movementController.AddVelocity(_currentAddedVelocity * -1);
            _currentAddedVelocity = Vector2.zero;
        }
    }
}