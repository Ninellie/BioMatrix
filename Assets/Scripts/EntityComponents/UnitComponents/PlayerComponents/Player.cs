using System;
using Assets.Scripts.Core.Variables;
using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _currentState;
        [SerializeField] private float _shootingAnimationSpeed = 0.5f;
        [SerializeField] private float _firearmCooldown = 0.2f;

        private void Awake()
        {
            if (_animator == null) _animator = GetComponent<Animator>();
        }

        private void PlayShootAnimation()
        {
            _animator.speed = _shootingAnimationSpeed;
            CancelInvoke(nameof(RestoreAnimationSpeed));
            Invoke(nameof(RestoreAnimationSpeed), _firearmCooldown);
        }

        private void RestoreAnimationSpeed()
        {
            _animator.speed = 1f;
        }

        private void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;
            _animator.Play(newState);
        }
    }

    public class ColliderRadiusUpdater : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private FloatReference _radius;

        private void Awake()
        {
            if (_circleCollider == null) _circleCollider = GetComponent<CircleCollider2D>();
        }

        public void UpdateRadius()
        {
            _circleCollider.radius = Math.Max(_radius, 0);
        }
    }

    public class ExperienceForNewLevelUpdater : MonoBehaviour
    {

    }

    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable _selfVariable;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Shield _shield;
        public Shield Shield => _shield;
        public bool IsFireButtonPressed { get; private set; }
        private const int ExperienceAmountIncreasingPerLevel = 16;

        private CircleCollider2D _circleCollider;

        private bool _isSubscribed;

        private bool _isCaged;
        private float _cageTime = 15f;

        private void Awake()
        {
            Debug.Log($"{gameObject.name} {nameof(Player)} Awake");
            _selfVariable.value = gameObject;
        }

        [ContextMenu(nameof(LevelUp))]
        private void LevelUp()
        {
            Debug.LogWarning("Level up"); 
            //var statMod = new StatMod(OperationType.Addition, ExperienceAmountIncreasingPerLevel); // TODO add this value to stats as stat
            //_stats.GetStat(StatName.ExperienceToNewLevel).AddModifier(statMod);
            //_resources.GetResource(ResourceName.Level).Increase();
            //_resources.GetResource(ResourceName.Experience).Empty();
            IsFireButtonPressed = false;
        }

    }
}