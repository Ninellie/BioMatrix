using System;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    [Serializable]
    public class ProjectileCollisionData
    {
        public string tag;
        public FloatReference damage;
        public FloatReference knockbackPower;
    }

    public class EnemyCollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _onDeathDrop;
        [SerializeField] private GameObject _damagePopup;
        [SerializeField] private bool _dieOnPlayerCollision;
        [SerializeField] private Color _takeDamageColor;

        [SerializeField] private Vector2Reference _playerPosition;
        [SerializeField] private Reserve _health;

        [SerializeField] private ProjectileCollisionData[] _projectileCollisionData;

        private bool _isAlive;
        private int _dropCount = 1;
        private bool _deathFromProjectile;

        private Color _spriteColor;
        private const float ReturnToDefaultColorSpeed = 5f;
        private const float OffscreenDieSeconds = 5f;

        private KnockbackController _knockbackController;
        private CircleCollider2D _circleCollider;
        private SpriteRenderer _spriteRenderer;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _isAlive = true;
            _knockbackController = GetComponent<KnockbackController>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _spriteColor = _spriteRenderer.color;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!_isAlive) return;
            var otherTag = collision2D.collider.tag;
            if (otherTag == "Player") CollideWithPlayer();
            foreach (var projectileCollisionData in _projectileCollisionData)
            {
                if (otherTag != projectileCollisionData.tag) continue;
                CollideWithProjectile(collision2D, projectileCollisionData.damage, projectileCollisionData.knockbackPower);
                return;
            }
        }

        private void Update() => BackToNormalColor();

        private void OnBecameInvisible() => StartDeathTimer();

        private void OnBecameVisible() => StopDeathTimer();

        private void CollideWithProjectile(Collision2D collision2D, int damage, float knockbackPower)
        {
            _deathFromProjectile = true;
            _health.TakeDamage(damage);
            var dropPosition = GetClosestPointOnCircle(collision2D.transform.position);
            DropDamagePopup(damage, dropPosition);

            ChangeColorOnDamageTaken();
            var force = ((Vector2)_transform.position - _playerPosition.Value); // Сложность в том, что враг отталкивается в противоположную сторону от игрока, а не от пули с которой он столкнулся
            force.Normalize();
            force *= knockbackPower;
            _knockbackController.Knockback(force);

            if (!_health.IsEmpty) return;
            _isAlive = false;
        }

        private void CollideWithPlayer()
        {
            if (!_isAlive) return;
            if (!_dieOnPlayerCollision) return;
            _deathFromProjectile = false;
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
            Instantiate(_onDeathDrop, _transform.position, new Quaternion(0, 0, 0, 0));
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
            var offset = _circleCollider.radius * direction.normalized;
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