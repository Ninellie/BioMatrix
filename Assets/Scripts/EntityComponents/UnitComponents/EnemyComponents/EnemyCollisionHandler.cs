using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class EnemyCollisionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _onDeathDrop;
        [SerializeField] private GameObject _damagePopup;
        [SerializeField] private bool _dieOnPlayerCollision;
        [SerializeField] private Color _takeDamageColor;

        [SerializeField] private FloatReference _currentHealth;

        [SerializeField] private FloatReference _playerProjectileDamage;
        [SerializeField] private FloatReference _playerProjectileKnockBack;

        private bool _isAlive;
        private int _dropCount = 1;
        private readonly Rarity _rarity = new();
        private bool _deathFromProjectile;
        
        private KnockbackController _knockbackController;
        private Rigidbody2D _rigidbody2D;
        private CircleCollider2D _circleCollider;
        private SpriteRenderer _spriteRenderer;

        private Color _spriteColor;
        private const float ReturnToDefaultColorSpeed = 5f;
        private const float OffscreenDieSeconds = 5f;

        private ISlayer _lastDamageSource;
        private Player _player;

        private bool _isSubscribed;

        private void Awake()
        {
            _isAlive = true;

            _knockbackController = GetComponent<KnockbackController>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _player = FindObjectOfType<Player>();

            _spriteColor = _spriteRenderer.color;
            _rarity.Value = RarityEnum.Normal;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!_isAlive) return;
            switch (collision2D.collider.tag)
            {
                case "PlayerProjectile":
                    CollideWithProjectile(collision2D);
                    break;
                case "Player":
                    CollideWithPlayer();
                    break;
            }
        }

        private void Update() => BackToNormalColor();

        private void OnBecameInvisible() => StartDeathTimer();

        private void OnBecameVisible() => StopDeathTimer();

        private void CollideWithProjectile(Collision2D collision2D)
        {
            var damage = (int)_playerProjectileDamage;
            _deathFromProjectile = true;
            TakeDamage(damage);

            var dropPosition = GetClosestPointOnCircle(collision2D.transform.position);
            DropDamagePopup(damage, dropPosition);

            ChangeColorOnDamageTaken();

            Vector2 force = transform.position - _player.transform.position;
            force.Normalize();
            force *= _playerProjectileKnockBack;
            _knockbackController.Knockback(force);

            if (_currentHealth == 0) return;
            _isAlive = false;
        }

        private void CollideWithPlayer()
        {
            if (!_isAlive) return;
            if (!_dieOnPlayerCollision) return;
            _deathFromProjectile = false;
            Death();
        }

        private void TakeDamage(int damage)
        {
            _currentHealth.constantValue -= damage;
            if (_currentHealth != 0) return;
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
            Instantiate(_onDeathDrop, _rigidbody2D.position, new Quaternion(0, 0, 0, 0));
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
            _lastDamageSource?.IncreaseKills();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}