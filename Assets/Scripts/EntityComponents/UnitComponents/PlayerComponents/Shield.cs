using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private ShieldStatsSettings _stats;
        [SerializeField] private LayerMask _resistancePhysLayer;
        [SerializeField] private GameObject _shield;
        [SerializeField] private float _spriteAlphaPerLayer = 0.2f;
        [SerializeField] private Color _shieldColor = Color.cyan;

        public Stat MaxLayers { get; private set; }
        public Stat MaxRechargeableLayers { get; private set; }
        public Stat RechargeRatePerSecond { get; private set; }
        public Stat RepulseForce { get; private set; }
        public Stat RepulseRadius { get; private set; }

        public RecoverableResource LayersCount { get; private set; }

        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _shieldSprite;

        private void Awake()
        {
            _shieldSprite = _shield.GetComponent<SpriteRenderer>();
            _capsuleCollider = GetComponentInParent<CapsuleCollider2D>();

            MaxLayers = StatFactory.GetStat(_stats.maxLayers);
            MaxRechargeableLayers = StatFactory.GetStat(_stats.maxRechargeableLayers);
            var rechargeRatePerSecond = _stats.rechargeRatePerMinute / 60f;
            RechargeRatePerSecond = StatFactory.GetStat(rechargeRatePerSecond);
            RepulseForce = StatFactory.GetStat(_stats.repulseForce);
            RepulseRadius = StatFactory.GetStat(_stats.repulseRadius);

            LayersCount = new RecoverableResource(0, MaxLayers, RechargeRatePerSecond, MaxRechargeableLayers);
        }

        private void OnEnable()
        {
            LayersCount.EmptyEvent += Disable;
            LayersCount.NotEmptyEvent += Enable;
            LayersCount.ValueChangedEvent += UpdateShieldAlpha;
            LayersCount.DecrementEvent += Repulse;
        }

        private void OnDisable()
        {
            LayersCount.EmptyEvent -= Disable;
            LayersCount.NotEmptyEvent -= Enable;
            LayersCount.ValueChangedEvent -= UpdateShieldAlpha;
            LayersCount.DecrementEvent -= Repulse;
        }

        private void Start()
        {
            var initialLayersCount = (int)MaxRechargeableLayers.Value;
            initialLayersCount = Mathf.Max(initialLayersCount, 0);
            LayersCount.Increase(initialLayersCount);
            UpdateShieldAlpha();

            if (initialLayersCount == 0)
                Disable();
            else
                Enable();
        }

        private void Update()
        {
            LayersCount.TimeToRecover += Time.deltaTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, RepulseRadius?.Value ?? _stats.repulseRadius);
        }

        private void Repulse()
        {
            //var nearbyEnemies = GetNearbyEnemiesList(RepulseRadius.Value, _resistancePhysLayer);
            var nearbyEnemies = GetNearbyEnemiesKnockbackControllersList(RepulseRadius.Value, _resistancePhysLayer);
            foreach (var enemy in nearbyEnemies)
            {
                enemy.Knockback(gameObject);
            }
            //enemy.enemyMoveController.KnockBackFromTarget(RepulseForce.Value);

            //TODO Lead to the next target.GetComponent<MovementController>().KnockBackFromTarget(force);
        }

        private List<EnemyComponents.Enemy> GetNearbyEnemiesList(float repulseRadius, LayerMask enemyLayer)
        {
            var colliders2D = Physics2D.OverlapCircleAll(transform.position, repulseRadius, enemyLayer);
            return colliders2D.Select(collider2d => collider2d.gameObject.GetComponent<EnemyComponents.Enemy>()).ToList();
        }

        private List<IKnockbackController> GetNearbyEnemiesKnockbackControllersList(float repulseRadius, LayerMask enemyLayer)
        {
            var colliders2D = Physics2D.OverlapCircleAll(transform.position, repulseRadius, enemyLayer);
            return colliders2D.Select(collider2d => collider2d.gameObject.GetComponent<IKnockbackController>()).ToList();
        }
    
        private void Disable()
        {
            _capsuleCollider.enabled = false;
            _shield.SetActive(false);
        }

        private void Enable()
        {
            _capsuleCollider.enabled = true;
            _shield.SetActive(true);
        }

        private void UpdateShieldAlpha()
        {
            var spriteAlpha = _spriteAlphaPerLayer * LayersCount.GetValue();
            _shieldColor.a = spriteAlpha;
            _shieldSprite.color = _shieldColor;
        }
    }
}