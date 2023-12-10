using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.View;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class ArmoredEnemyCollisionHandler : MonoBehaviour
    {
        [SerializeField] private bool _isArmored;
        [SerializeField] private int _damageThreshold;
        [SerializeField] private float _armoredTime;
        [Space]
        [SerializeField] private GameObjectReference _player;
        [SerializeField] private GameObject _onDeathDrop;
        [SerializeField] private GameObject _damagePopup;
        [SerializeField] private bool _dieOnPlayerCollision;
        [SerializeField] private Color _takeDamageColor;
        [Space]
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Space]
        [SerializeField] private MovementController _movementController;
        [SerializeField] private KnockbackController _knockbackController;
        [SerializeField] private Reserve _reserve;
        [Space]
        [SerializeField] private StatReference _playerProjectileDamage;
        [SerializeField] private StatReference _playerProjectileKnockback;
        [SerializeField] private StatReference _turretProjectileDamage;
        [SerializeField] private StatReference _turretProjectileKnockback;
        [Space]
        [SerializeField] private UnityEvent<string, Collision2D> _onHit = new();

        private bool _isAlive;
        private int _dropCount = 1;
        private bool _deathFromProjectile;
        private Color _spriteColor;
        private const float ReturnToDefaultColorSpeed = 5f;
        private const float OffscreenDieSeconds = 5f;

        private void Awake()
        {
            _isAlive = true;
            _isArmored = false;

            if (_circleCollider == null) _circleCollider = GetComponent<CircleCollider2D>();
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_movementController == null) _movementController = GetComponent<MovementController>();
            if (_knockbackController == null) _knockbackController = GetComponent<KnockbackController>();
            if (_reserve == null) _reserve = GetComponent<Reserve>();

            _spriteColor = _spriteRenderer.color;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!_isAlive) return;
            var otherTag = collision2D.collider.tag;
            _onHit.Invoke(otherTag, collision2D);
            switch (otherTag)
            {
                case "Projectile":
                    CollideWithProjectile(collision2D.gameObject.transform.position);
                    break;
                case "TurretProjectile":
                    CollideWithTurretProjectile(collision2D.gameObject.transform.position);
                    break;
                case "Player":
                    CollideWithPlayer();
                    break;
            }
        }

        private void Update() => BackToNormalColor();

        private void OnBecameInvisible() => StartDeathTimer();

        private void OnBecameVisible() => StopDeathTimer();

        private void BecomeArmored()
        {
            _movementController.SetSpeedScale(0);
            _isArmored = true;
            Invoke(nameof(RemoveArmored), _armoredTime);
        }

        private void RemoveArmored()
        {
            _isArmored = false;
        }

        private void CollideWithProjectile(Vector2 projectilePosition)
        {
            var projectileDamage = (int)_playerProjectileDamage;

            if (!_isArmored)
            {
                BecomeArmored();
            }
            else
            {
                projectileDamage = projectileDamage < _damageThreshold ? 0 : 1;
            }

            if (projectileDamage > 0)
            {
                _deathFromProjectile = true;
            }

            TakeDamage(projectileDamage);

            var dropPosition = GetClosestPointOnCircle(projectilePosition);
            DropDamagePopup(projectileDamage, dropPosition);
            ChangeColorOnDamageTaken();

            Vector2 force = transform.position - _player.Value.transform.position;
            force.Normalize();
            force *= _playerProjectileKnockback;
            _knockbackController.Knockback(force);
            if (!_reserve.IsEmpty) return;
            _isAlive = false;
        }

        private void CollideWithTurretProjectile(Vector2 projectilePosition)
        {
            var projectileDamage = (int)_turretProjectileDamage;

            if (!_isArmored)
            {
                BecomeArmored();
            }
            else
            {
                projectileDamage = projectileDamage < _damageThreshold ? 0 : 1;
            }

            if (projectileDamage > 0)
            {
                _deathFromProjectile = true;
            }

            TakeDamage(projectileDamage);

            var dropPosition = GetClosestPointOnCircle(projectilePosition);
            DropDamagePopup(projectileDamage, dropPosition);
            ChangeColorOnDamageTaken();

            Vector2 force = transform.position - _player.Value.transform.position;
            force.Normalize();
            force *= _turretProjectileKnockback;
            _knockbackController.Knockback(force);
            if (!_reserve.IsEmpty) return;
            _isAlive = false;
        }

        private void CollideWithPlayer()
        {
            if (!_isAlive) return;
            if (!_dieOnPlayerCollision) return;
            _deathFromProjectile = false;
            Death();
        }

        private void TakeDamage(int damageAmount)
        {
            _reserve.TakeDamage(damageAmount);
            if (!_reserve.IsEmpty) return;
            Death();
        }

        private void StartDeathTimer()
        {
            Debug.LogWarning(nameof(StartDeathTimer));
            _deathFromProjectile = false;
            Invoke(nameof(Death), OffscreenDieSeconds);
        }

        private void StopDeathTimer()
        {
            Debug.LogWarning(nameof(StopDeathTimer));
            CancelInvoke(nameof(Death));
        }

        private void BackToNormalColor()
        {
            if (_spriteRenderer.color == Color.white) return;
            var colorStep = ReturnToDefaultColorSpeed * Time.deltaTime;
            _spriteColor = _spriteRenderer.color;
            _spriteColor.r += colorStep;
            _spriteColor.g += colorStep;
            _spriteColor.b += colorStep;
            _spriteRenderer.color = _spriteColor;
        }

        private void ChangeColorOnDamageTaken()
        {
            _spriteRenderer.color = _takeDamageColor;
        }

        private void DropBonus()
        {
            if (_dropCount <= 0) return;
            _dropCount--;
            Instantiate(_onDeathDrop, transform.position, new Quaternion(0, 0, 0, 0));
        }

        private void DropDamagePopup(int damageValue, Vector2 position)
        {
            var droppedDamagePopup = Instantiate(_damagePopup);
            droppedDamagePopup.transform.position = position;
            droppedDamagePopup.GetComponent<DamagePopup>().Setup(damageValue);
        }

        private Vector2 GetClosestPointOnCircle(Vector2 otherCircleColliderCenter)
        {
            Vector2 center = _circleCollider.transform.position;
            var direction = otherCircleColliderCenter - center;
            direction.Normalize();
            var radius = _circleCollider.radius;
            var offset = radius * direction.normalized;
            return center + offset;
        }

        private void Death()
        {
            Debug.Log($"Enemy {gameObject.name} died. From projectile: {_deathFromProjectile}");
            if (_deathFromProjectile) DropBonus();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}