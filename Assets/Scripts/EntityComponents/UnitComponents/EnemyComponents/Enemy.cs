using System;
using Assets.Scripts.Core.Render;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using Assets.Scripts.EntityComponents.UnitComponents.ProjectileComponents;
using Assets.Scripts.View;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public interface IDamageDealer
    {
        int GetDamage();
    }

    public interface IDamageable
    {
        void TakeDamage(int damageAmount);
    }

    public interface ISpawning
    {
        float GetSpawningWeight();
    }

    public class Enemy : MonoBehaviour, ISpawning, IDamageDealer, IDamageable, IKnockbackDealer
    {
        [SerializeField] private GameObject _onDeathDrop;
        [SerializeField] private GameObject _damagePopup;
        [SerializeField] private EnemyType _enemyType = EnemyType.SideView;
        [SerializeField] private bool _dieOnPlayerCollision;

        public KnockbackController knockbackController;
        public bool IsAlive => _isAlive;
        private int _dropCount = 1;
        private readonly Rarity _rarity = new();
        private bool _deathFromProjectile;

        private Rigidbody2D _rigidbody2D;
        private SpriteOutline _spriteOutline;
        private CircleCollider2D _circleCollider;
        private SpriteRenderer _spriteRenderer;
        private StatList _stats;
        private ResourceList _resources;

        private Color _spriteColor;
        private const float ReturnToDefaultColorSpeed = 5f;
        private const long OffscreenDieSeconds = 60;

        private ISlayer _lastDamageSource;
        private Player _player;

        private bool _isAlive;
        private bool _isSubscribed;


        private void Awake()
        {
            Debug.Log($"{gameObject.GetInstanceID()} Enemy Awake");
            _isAlive = true;

            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            knockbackController = GetComponent<KnockbackController>();
            _spriteOutline = GetComponent<SpriteOutline>();
            _circleCollider = GetComponent<CircleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _player = FindObjectOfType<Player>(); //!!

            _spriteColor = _spriteRenderer.color;
            _rarity.Value = RarityEnum.Normal;

        }

        private void Start()
        {
            Subscribe();
            UpdateCurrentSize();
        }

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!_isAlive) return;

            var otherTag = collision2D.collider.tag;

            switch (otherTag)
            {
                case "Enemy":
                    return;
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

        private void OnBecameInvisible() => StartDeathTimer();

        private void OnBecameVisible() => StopDeathTimer();

        private void StartDeathTimer()
        {
            _deathFromProjectile = false;
            Invoke(nameof(Death), OffscreenDieSeconds);
        }

        private void StopDeathTimer() => CancelInvoke(nameof(Death));

        private void Subscribe()
        {
            if (_isSubscribed) return;

            var sizeStat = _stats.GetStat(StatName.Size);
            var healthResource = _resources.GetResource(ResourceName.Health);

            if (sizeStat is null) return;
            if (healthResource is null) return;

            _stats.GetStat(StatName.Size).valueChangedEvent.AddListener(UpdateCurrentSize);
            _resources.GetResource(ResourceName.Health).AddListenerToEvent(ResourceEventType.Empty, Death);
            //_resources.GetResource(ResourceName.Health).OnEmpty.AddListener(Death);

            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;

            _stats.GetStat(StatName.Size).valueChangedEvent.RemoveListener(UpdateCurrentSize);
            _resources.GetResource(ResourceName.Health).RemoveListenerToEvent(ResourceEventType.Empty, Death);

            _isSubscribed = false;
        }

        private void CollideWithProjectile(Projectile projectile)
        {
            var health = _resources.GetResource(ResourceName.Health);

            var projectileDamage = projectile.GetDamage();
            if (projectileDamage > 0) _lastDamageSource = projectile;
            _deathFromProjectile = true;
            TakeDamage(projectileDamage);

            var dropPosition = GetClosestPointOnCircle(projectile.transform.position); //TODO move to Circle.cs
            DropDamagePopup(projectileDamage, dropPosition);

            ChangeColorOnDamageTaken();

            Vector2 force = transform.position - _player.transform.position;
            force.Normalize();
            force *= projectile.GetKnockbackPower();
            knockbackController.Knockback(force);

            if (!health.IsEmpty) return;
            //Death();
            _isAlive = false;
        }

        private void CollideWithPlayer(Player player)
        {
            if (!_isAlive) return;
            _deathFromProjectile = false;
            if (!_dieOnPlayerCollision) return;
            _lastDamageSource = player;
            Death();
        }

        //private void CollideWithEnemy(Enemy enemy)
        //{
        //    return;
        //}


        private void Update()
        {
            BackToNormalColor();
        }

        public int GetDamage()
        {
            return (int)_stats.GetStat(StatName.Damage).Value;
        }

        public void TakeDamage(int damageAmount)
        {
            _resources.GetResource(ResourceName.Health).Decrease(damageAmount);
        }

        public void LookAt2D(Vector2 target)
        {
            var direction = (Vector2)_rigidbody2D.transform.position - target;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            _rigidbody2D.rotation = angle;
            _rigidbody2D.SetRotation(angle);
        }

        public void SetRarity(RarityEnum rarityEnum)
        {
            _rarity.Value = rarityEnum;
            if (rarityEnum == RarityEnum.Normal) return;

            var color = _rarity.Color;
            var multiplier = _rarity.Multiplier;

            _spriteOutline.enabled = true;
            _spriteOutline.color = color;
        
            var statMod = new StatMod(OperationType.Multiplication, multiplier);
            _stats.GetStat(StatName.MaximumHealth).AddModifier(statMod);
        }
        
        public EnemyType GetEnemyType() => _enemyType;

        public float GetSpawningWeight() => _stats.GetStat(StatName.SpawnWeight).Value;

        public float GetKnockbackPower() => _stats.GetStat(StatName.KnockbackPower).Value;

        private void BackToNormalColor()
        {
            if (_spriteRenderer.color == Color.white) return;
            _spriteColor = _spriteRenderer.color;
            _spriteColor.r += ReturnToDefaultColorSpeed * Time.deltaTime;
            _spriteColor.g += ReturnToDefaultColorSpeed * Time.deltaTime;
            _spriteColor.b += ReturnToDefaultColorSpeed * Time.deltaTime;
            _spriteRenderer.color = _spriteColor;
        }

        private void ChangeColorOnDamageTaken()
        {
            _spriteRenderer.color = _enemyType switch
            {
                EnemyType.AboveView => Color.cyan,
                EnemyType.SideView => Color.red,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void DropBonus()
        {
            _dropCount--;
            var rotation = new Quaternion(0, 0, 0, 0);
            Instantiate(_onDeathDrop, _rigidbody2D.position, rotation);
        }
        
        private void DropDamagePopup(int damage, Vector2 position)
        {
            var droppedDamagePopup = Instantiate(_damagePopup);
            droppedDamagePopup.transform.position = position;
            droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
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

        private void UpdateCurrentSize()
        {
            var sizeValue = _stats.GetStat(StatName.Size).Value;
            transform.localScale = new Vector3(sizeValue, sizeValue, 1);
        }

        private void Death()
        {
            Debug.LogWarning("Death of enemy");

            if (_deathFromProjectile) DropBonus();
            _lastDamageSource.IncreaseKills();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}