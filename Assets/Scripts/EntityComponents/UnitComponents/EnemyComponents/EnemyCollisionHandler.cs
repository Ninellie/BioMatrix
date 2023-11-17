using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
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

        [SerializeField] private FloatReference currentHealth;
        [field: SerializeField] public FloatReference CollisionDamage { get; private set; }

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
                case "Projectile":
                    var projectile = collision2D.gameObject.GetComponent<Projectile>();
                    CollideWithProjectile(projectile);
                    break;
                case "Player":
                    var player = collision2D.gameObject.GetComponent<Player>();
                    CollideWithPlayer(player);
                    break;
            }
        }

        private void Update() => BackToNormalColor();

        private void OnBecameInvisible() => StartDeathTimer();

        private void OnBecameVisible() => StopDeathTimer();

        private void CollideWithProjectile(Projectile projectile)
        {
            var projectileStats = projectile.GetComponent<StatList>();
            var projectileDamage = (int)projectileStats.GetStat(StatName.Damage).Value;
            if (projectileDamage > 0) _lastDamageSource = projectile;
            _deathFromProjectile = true;
            TakeDamage(projectileDamage);

            var dropPosition = GetClosestPointOnCircle(projectileStats.transform.position);
            DropDamagePopup(projectileDamage, dropPosition);

            ChangeColorOnDamageTaken();

            Vector2 force = transform.position - _player.transform.position;
            force.Normalize();
            force *= projectileStats.GetStat(StatName.KnockbackPower).Value;
            _knockbackController.Knockback(force);

            if (currentHealth == 0) return;
            _isAlive = false;
        }

        private void CollideWithPlayer(ISlayer player)
        {
            if (!_isAlive) return;
            if (!_dieOnPlayerCollision) return;
            _deathFromProjectile = false;
            _lastDamageSource = player;
            Death();
        }

        private void TakeDamage(int damage)
        {
            currentHealth.constantValue -= damage;
            if (currentHealth != 0) return;
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