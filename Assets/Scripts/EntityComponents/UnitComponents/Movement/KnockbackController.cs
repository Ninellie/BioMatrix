using System.Collections;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Movement
{
    public class KnockbackController : MonoBehaviour, IKnockbackController
    {
        [SerializeField]
        [Tooltip("In points per seconds")]
        private float _knockbackSpeed;

        private bool _isActive = false;
        private bool _isSecondaryActive = false;
        private Vector2 MyPosition => transform.position;
        private IMovementController _movementController;
        
        protected void Awake()
        {
            _movementController = GetComponent<IMovementController>();
        }

        public void Knockback(Vector2 force)
        {
            var knockbackTime = force.magnitude / _knockbackSpeed;
            var knockbackDirection = force.normalized;
            var knockbackVelocity = knockbackDirection * _knockbackSpeed;
            StartCoroutine(InitKnockback(knockbackTime, knockbackVelocity));
        }

        public void Knockback(GameObject target)
        {
            var force = MyPosition - (Vector2)target.transform.position;
            force.Normalize();
            force *= target.GetComponent<Entity>().KnockbackPower.Value;
            Knockback(force);
        }

        private IEnumerator InitKnockback(float time, Vector2 velocity)
        {
            bool isSecondary;

            if (_isActive)
            {
                isSecondary = true;
                _isSecondaryActive = true;
            }
            else
            {
                isSecondary = false;
                _isSecondaryActive = false;
                _isActive = true;
                _movementController.AddVelocity(velocity);
            }

            _movementController.SetSpeedScale(0);
            
            yield return new WaitForSeconds(time);

            if (_isSecondaryActive && isSecondary)
            {
                _isSecondaryActive = false;
                _isActive = false;
                _movementController.AddVelocity(velocity * -1);
            }
            else if (!_isSecondaryActive && !isSecondary)
            {
                _isActive = false;
                _movementController.AddVelocity(velocity * -1);
            }
        }
    }
}