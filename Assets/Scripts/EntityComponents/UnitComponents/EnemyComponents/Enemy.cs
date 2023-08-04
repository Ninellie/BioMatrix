using System;
using Assets.Scripts.Core.Render;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Enemy;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using Assets.Scripts.GameSession.UIScripts.View;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Enemy : Unit
    {
        [SerializeField] private GameObject _onDeathDrop;
        [SerializeField] private GameObject _damagePopup;
        [SerializeField] private EnemyType _enemyType = EnemyType.SideView;
        [SerializeField] private bool _dieOnPlayerCollision;
        public EnemyStatsSettings Settings => GetComponent<EnemyStatsSettings>();
        public KnockbackController knockbackController;

        //public EnemyMoveController enemyMoveController;

        private int _dropCount = 1;
        private readonly Rarity _rarity = new Rarity();
        private SpriteOutline _spriteOutline;
        private bool _deathFromProjectile = false;
        private CircleCollider2D _circleCollider;
        private Color _spriteColor;
        private float _deathTimer;
        private const float ReturnToDefaultColorSpeed = 5f;
        private const int MinInitialLevel = 1;
        private const float MaxLifeIncreasePerLevel = 0;
        private const long OffscreenDieSeconds = 60;

        private Entity _lastDamageSource;
        private PlayerComponents.Player _player;

        private int _level;
        private int Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    if (value >= MinInitialLevel)
                    {
                        _level = value;
                    }
                }
                UpdateMaxLifeStat();
            }
        }
        private void OnEnable() => BaseOnEnable();

        private void OnDisable() => BaseOnDisable();

        private void Awake() => BaseAwake(Settings);

        private void FixedUpdate() => BaseFixedUpdate();
        
        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (!Alive) return;
            _lastDamageSource = this;
            var otherCollider2D = collision2D.collider;
        
            if (otherCollider2D.gameObject.CompareTag("Enemy"))
            {
                return;
            }

            if (!otherCollider2D.gameObject.TryGetComponent<Entity>(out Entity entity))
            {
                Debug.LogWarning("OnTriggerEnter2D with game object without Entity component");
                return;
            }

            var collisionEntity = otherCollider2D.gameObject.GetComponent<Entity>();

            if (otherCollider2D.gameObject.CompareTag("Player"))
            {
                if (_dieOnPlayerCollision)
                {
                    _deathFromProjectile = false;
                    Death();
                }
            }
            if (!otherCollider2D.gameObject.CompareTag("Projectile")) return;

            var projectileDamage = collisionEntity.Damage.Value;
            if(projectileDamage > 0) _lastDamageSource = collisionEntity;
            _deathFromProjectile = true;
            TakeDamage(projectileDamage);

            var isKilled = false;
            if (LifePoints.IsEmpty)
            {
                isKilled = true;
                _lastDamageSource.KillsCount.Increase();
            }

            var position = GetClosestPointOnCircle(otherCollider2D as CircleCollider2D); //TODO move to Circle.cs
            DropDamagePopup((int)projectileDamage, position);

            if (isKilled)
            {
                return;
            }

            ChangeColorOnDamageTaken();

            Vector2 force = (transform.position - _player.transform.position);
            force.Normalize();
            force *= collisionEntity.KnockbackPower.Value;

            knockbackController.Knockback(force);
        }

        private void Update() => BaseUpdate();

        protected void BaseAwake(EnemyStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} Enemy Awake");
            base.BaseAwake(settings);

            _spriteOutline = GetComponent<SpriteOutline>();
            _circleCollider = GetComponent<CircleCollider2D>();

            _spriteColor = SpriteRenderer.color;
            _rarity.Value = RarityEnum.Normal;
            Level = MinInitialLevel;
            RestoreLifePoints();

            _player = FindObjectOfType<PlayerComponents.Player>(); //!!

            knockbackController = GetComponent<KnockbackController>();
        }

        protected void BaseFixedUpdate()
        {
            DeathTimerFixedUpdate();
            //enemyMoveController.FixedUpdateMoveStep();
        }
        
        protected override void BaseUpdate()
        {
            base.BaseUpdate();
            BackToNormalColor();
        }

        public void LookAt2D(Vector2 target)
        {
            var direction = (Vector2)Rb2D.transform.position - target;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            Rb2D.rotation = angle;
            Rb2D.SetRotation(angle);
        }

        public void SetRarity(RarityEnum rarityEnum)
        {
            _rarity.Value = rarityEnum;
            if (rarityEnum == RarityEnum.Normal) return;

            var color = _rarity.Color;
            var multiplier = _rarity.Multiplier;

            _spriteOutline.enabled = true;
            _spriteOutline.color = color;
        
            var statMod = new StatModifier(OperationType.Multiplication, multiplier);

            MaximumLifePoints.AddModifier(statMod);
        }
        
        public void LevelUp(int value)
        {
            if(value < 0) return;
            Level += value;
        }
        
        public EnemyType GetEnemyType()
        {
            return _enemyType;
        }
        
        protected override void Death()
        {
            if (_deathFromProjectile)
            {
                DropBonus();
            }

            base.Death();
        }

        private void BackToNormalColor()
        {
            if (SpriteRenderer.color == Color.white) return;
            _spriteColor = SpriteRenderer.color;
            _spriteColor.r += ReturnToDefaultColorSpeed * Time.deltaTime;
            _spriteColor.g += ReturnToDefaultColorSpeed * Time.deltaTime;
            _spriteColor.b += ReturnToDefaultColorSpeed * Time.deltaTime;
            SpriteRenderer.color = _spriteColor;
        }

        private void DeathTimerFixedUpdate()
        {
            if (IsOnScreen)
            {
                _deathTimer = 0;
                return;
            }
            if (Time.timeScale == 0) return;
            _deathTimer += Time.fixedDeltaTime;
            if (!(_deathTimer >= OffscreenDieSeconds)) return;
            _deathFromProjectile = false;
            TakeDamage(LifePoints.GetValue());
        }

        private void ChangeColorOnDamageTaken()
        {
            SpriteRenderer.color = _enemyType switch
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
            Instantiate(_onDeathDrop, Rb2D.position, rotation);
        }
        
        private void DropDamagePopup(int damage, Vector2 position)
        {
            var droppedDamagePopup = Instantiate(_damagePopup);
            droppedDamagePopup.transform.position = position;
            droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
        }
        
        private void UpdateMaxLifeStat()
        {
            if (Level <= 1) return;
            var addingValue = (_level - 1) * MaxLifeIncreasePerLevel;
            var mod = new StatModifier(OperationType.Addition, addingValue);
            MaximumLifePoints.AddModifier(mod);
        }

        private Vector2 GetClosestPointOnCircle(CircleCollider2D otherCollider)
        {
            Vector2 center = _circleCollider.transform.position;
            Vector2 otherCenter = otherCollider.transform.position;
            var direction = otherCenter - center;
            direction.Normalize();
            var radius = _circleCollider.radius;
            var offset = radius * direction.normalized;
            return center + offset;
        }
    }
}