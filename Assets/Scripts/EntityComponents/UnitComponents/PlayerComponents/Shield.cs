using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private LayerMask _resistancePhysLayer;
        [SerializeField] private float _spriteAlphaPerLayer = 0.2f;
        [SerializeField] private Color _color = Color.cyan;

        [SerializeField] private StatList _statList;

        public OldStat MaxLayers { get; private set; }
        public OldStat MaxRechargeableLayers { get; private set; }
        public OldStat RechargeRatePerSecond { get; private set; }
        public OldStat RepulseForce { get; private set; }
        public OldStat RepulseRadius { get; private set; }

        public OldRecoverableResource LayersCount { get; private set; }

        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _shieldSprite;

        private void Awake()
        {
            _statList = GetComponent<StatList>();
            _shieldSprite = GetComponent<SpriteRenderer>();
            _capsuleCollider = GetComponentInParent<CapsuleCollider2D>();
        }

        private void Start()
        {
            MaxLayers = StatFactory.GetStat(_statList.GetStatByName(StatName.MaximumLayers).Value);
            MaxRechargeableLayers = StatFactory.GetStat(_statList.GetStatByName(StatName.MaximumRechargeableLayers).Value);
            var rechargeRatePerSecond = _statList.GetStatByName(StatName.RechargeRatePerMinute).Value / 60f;
            RechargeRatePerSecond = StatFactory.GetStat(rechargeRatePerSecond);
            RepulseForce = StatFactory.GetStat(_statList.GetStatByName(StatName.RepulseForce).Value);
            RepulseRadius = StatFactory.GetStat(_statList.GetStatByName(StatName.RepulseRadius).Value);

            LayersCount = new OldRecoverableResource(0, MaxLayers, RechargeRatePerSecond, MaxRechargeableLayers);

            var initialLayersCount = (int)MaxRechargeableLayers.Value;
            initialLayersCount = Mathf.Max(initialLayersCount, 0);
            LayersCount.Increase(initialLayersCount);
            UpdateShieldAlpha();
            
            Subscribe();

            if (initialLayersCount == 0)
                Disable();
            else
                Enable();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Update()
        {
            LayersCount.TimeToRecover += Time.deltaTime;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            if (RepulseRadius != null)
                Gizmos.DrawWireSphere(transform.position, RepulseRadius?.Value ?? RepulseRadius.Value);
        }

        private void Subscribe()
        {
            if (LayersCount == null) return;
            LayersCount.EmptyEvent += Disable;
            LayersCount.NotEmptyEvent += Enable;
            LayersCount.ValueChangedEvent += UpdateShieldAlpha;
            LayersCount.DecrementEvent += Repulse;
        }

        private void Unsubscribe()
        {
            if (LayersCount == null) return;
            LayersCount.EmptyEvent -= Disable;
            LayersCount.NotEmptyEvent -= Enable;
            LayersCount.ValueChangedEvent -= UpdateShieldAlpha;
            LayersCount.DecrementEvent -= Repulse;
        }

        private void Repulse()
        {
            var nearbyEnemies = GetNearbyEnemiesList(RepulseRadius.Value, _resistancePhysLayer);
            foreach (var enemy in nearbyEnemies)
            {
                var force = (Vector2)enemy.transform.position - (Vector2)gameObject.transform.position;
                force.Normalize();
                force *= RepulseForce.Value;
                enemy.knockbackController.Knockback(force);
            }
        }

        private List<EnemyComponents.Enemy> GetNearbyEnemiesList(float repulseRadius, LayerMask enemyLayer)
        {
            var colliders2D = Physics2D.OverlapCircleAll(transform.position, repulseRadius, enemyLayer);
            return colliders2D.Select(collider2d => collider2d.gameObject.GetComponent<EnemyComponents.Enemy>()).ToList();
        }

        private List<KnockbackController> GetNearbyEnemiesKnockbackControllersList(float repulseRadius, LayerMask enemyLayer)
        {
            var colliders2D = Physics2D.OverlapCircleAll(transform.position, repulseRadius, enemyLayer);
            return colliders2D.Select(collider2d => collider2d.gameObject.GetComponent<KnockbackController>()).ToList();
        }
    
        private void Disable()
        {
            _capsuleCollider.enabled = false;
            _shieldSprite.enabled = false;
        }

        private void Enable()
        {
            _capsuleCollider.enabled = true;
            _shieldSprite.enabled = true;
        }

        private void UpdateShieldAlpha()
        {
            var spriteAlpha = _spriteAlphaPerLayer * LayersCount.GetValue();
            _color.a = spriteAlpha;
            _shieldSprite.color = _color;
        }
    }
}