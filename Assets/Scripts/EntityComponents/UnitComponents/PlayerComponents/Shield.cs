using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.Knockback;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private LayerMask _resistancePhysLayer;
        [SerializeField] private float _spriteAlphaPerLayer = 0.2f;
        [SerializeField] private Color _color = Color.cyan;
        [SerializeField] private StatList _stats;
        [SerializeField] private ResourceList _resources;
        public Resource Layers { get; private set; }

        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _shieldSprite;

        private bool _isSubscribed;

        private void Awake()
        {
            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            _shieldSprite = GetComponent<SpriteRenderer>();
            _capsuleCollider = GetComponentInParent<CapsuleCollider2D>();
        }

        private void Start()
        {
            Layers = _resources.GetResource(ResourceName.Layers);
            UpdateShieldAlpha();
            Subscribe();

            if (Layers.GetValue() == 0)
                Disable();
            else
                Enable();
        }

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            var repulseRadius = _stats.GetStat(StatName.RepulseRadius).Value;
            Gizmos.DrawWireSphere(transform.position, repulseRadius);
        }

        private void Subscribe()
        {
            if (_isSubscribed) return;
            if (Layers == null) return;
            Layers.AddListenerToEvent(ResourceEventType.Empty, Disable);
            Layers.AddListenerToEvent(ResourceEventType.NotEmpty, Enable);
            Layers.AddListenerToEvent(ResourceEventType.ValueChanged, UpdateShieldAlpha);
            Layers.AddListenerToEvent(ResourceEventType.Decrement, Repulse);
            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;
            if (Layers == null) return;
            Layers.RemoveListenerToEvent(ResourceEventType.Empty, Disable);
            Layers.RemoveListenerToEvent(ResourceEventType.NotEmpty, Enable);
            Layers.RemoveListenerToEvent(ResourceEventType.ValueChanged, UpdateShieldAlpha);
            Layers.RemoveListenerToEvent(ResourceEventType.Decrement, Repulse);
            _isSubscribed = false;
        }

        private void Repulse()
        {
            var repulseRadius = _stats.GetStat(StatName.RepulseRadius).Value;
            var repulseForce = _stats.GetStat(StatName.RepulseForce).Value;
            var nearbyEnemies = GetNearbyEnemiesKnockbackControllerList(repulseRadius, _resistancePhysLayer);
            foreach (var enemy in nearbyEnemies)
            {
                var force = (Vector2)enemy.transform.position - (Vector2)gameObject.transform.position;
                force.Normalize();
                force *= repulseForce;
                enemy.Knockback(force);
            }
        }

        private List<KnockbackController> GetNearbyEnemiesKnockbackControllerList(float repulseRadius, LayerMask enemyLayer)
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
            var spriteAlpha = _spriteAlphaPerLayer * Layers.GetValue();
            _color.a = spriteAlpha;
            _shieldSprite.color = _color;
        }
    }
}