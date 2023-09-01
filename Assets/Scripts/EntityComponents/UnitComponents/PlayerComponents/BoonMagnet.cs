using Assets.Scripts.CustomAttributes;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.BoonComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class BoonMagnet : MonoBehaviour
    {
        [SerializeField] private LayerMask _boonLayer;
        [ReadOnly, SerializeField] private float _magnetismRadius;
        private Bounds _pickingBounds;

        private BoxCollider2D _boxCollider2D;

        private StatList _stats;
        private ResourceList _resources;
        private void Awake()
        {
            _stats = GetComponent<StatList>();
            _resources = GetComponent<ResourceList>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _pickingBounds = _boxCollider2D.bounds;
        }

        private void Start() => SetMagnetismRadius(); 
        private void OnEnable() => Subscribe();
        private void OnDisable() => UnSubscribe();
        private void Subscribe() => _stats.GetStat(StatName.MagnetismRadius).valueChangedEvent.AddListener(SetMagnetismRadius);
        private void UnSubscribe() => _stats.GetStat(StatName.MagnetismRadius).valueChangedEvent.RemoveListener(SetMagnetismRadius);
        private void SetMagnetismRadius() => _magnetismRadius = _stats.GetStat(StatName.MagnetismRadius).Value;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (!_boxCollider2D.IsTouchingLayers(_boonLayer)) return;
            if (!_boxCollider2D.IsTouching(collider2D)) return;
            var experienceAmount = collider2D.gameObject.GetComponent<Boon>().GetExperience();
            IncreaseExperience(experienceAmount);
        }

        private void IncreaseExperience(int value)
        {
            var expMultiplierValue = _stats.GetStat(StatName.ExperienceMultiplier).Value;
            var expTakenAmount = (int)(value * expMultiplierValue);
            _resources.GetResource(ResourceName.Experience).Increase(expTakenAmount);
        }

        //private void GetDirectionToNearestEnemy(float searchRadius)
        //{
        //    var collidersInAimingRadius = Physics2D.OverlapCircleAll(transform.position, searchRadius, _boonLayer);
        //}
    }
}