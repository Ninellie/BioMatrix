using Core.Variables.References;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _currentState = "Idle";
        [SerializeField] private FloatReference _shootingAnimationSpeed;// = 0.5f;
        [SerializeField] private FloatReference _firearmAttackSpeed;// = 0.2f;

        private float FirearmCooldown => 1 / _firearmAttackSpeed;

        private void Awake()
        {
            if (_animator == null) _animator = GetComponent<Animator>();
        }

        // on REAL shoot
        public void PlayShootAnimation()
        {
            _animator.speed = _shootingAnimationSpeed;
            CancelInvoke(nameof(RestoreAnimationSpeed));
            Invoke(nameof(RestoreAnimationSpeed), FirearmCooldown);
        }

        // on Move
        public void PlayRunAnimation()
        {
            if (_currentState == "Run") return;
            _currentState = "Run";
            _animator.Play(_currentState);
        }

        // on idle
        public void PlayIdleAnimation()
        {
            if (_currentState == "Idle") return;
            _currentState = "Idle";
            _animator.Play(_currentState);
        }

        private void RestoreAnimationSpeed()
        {
            _animator.speed = 1f;
        }
    }
}