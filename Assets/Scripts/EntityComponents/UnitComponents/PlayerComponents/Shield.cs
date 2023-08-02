using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private LayerMask _resistancePhysLayer;
        [SerializeField] private float _spriteAlphaPerLayer = 0.2f;
        [SerializeField] private Color _color = Color.cyan;

        [SerializeField] private StatHandler _statHandler;

        public OldStat MaxLayers { get; private set; }
        public OldStat MaxRechargeableLayers { get; private set; }
        public OldStat RechargeRatePerSecond { get; private set; }
        public OldStat RepulseForce { get; private set; }
        public OldStat RepulseRadius { get; private set; }

        public RecoverableResource LayersCount { get; private set; }

        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _shieldSprite;

        private void Awake()
        {
            _statHandler = GetComponent<StatHandler>();
            _shieldSprite = GetComponent<SpriteRenderer>();
            _capsuleCollider = GetComponentInParent<CapsuleCollider2D>();
        }

        private void Start()
        {
            MaxLayers = StatFactory.GetStat(_statHandler.GetStatByName("MaxLayers").Value);
            MaxRechargeableLayers = StatFactory.GetStat(_statHandler.GetStatByName("MaxRechargeableLayers").Value);
            var rechargeRatePerSecond = _statHandler.GetStatByName("RechargeRatePerMinute").Value / 60f;
            RechargeRatePerSecond = StatFactory.GetStat(rechargeRatePerSecond);
            RepulseForce = StatFactory.GetStat(_statHandler.GetStatByName("RepulseForce").Value);
            RepulseRadius = StatFactory.GetStat(_statHandler.GetStatByName("RepulseRadius").Value);

            LayersCount = new RecoverableResource(0, MaxLayers, RechargeRatePerSecond, MaxRechargeableLayers);

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
            var nearbyEnemies = GetNearbyEnemiesKnockbackControllersList(RepulseRadius.Value, _resistancePhysLayer);
            foreach (var enemy in nearbyEnemies)
                enemy.Knockback(gameObject);
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