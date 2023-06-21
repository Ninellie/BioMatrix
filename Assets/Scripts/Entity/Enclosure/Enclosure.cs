using Assets.Scripts.Entity.Stats;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Entity.Enclosure
{
    [RequireComponent(typeof(EnclosureStatsSettings))]
    [RequireComponent(typeof(Tilemap))]
    public class Enclosure : Entity
    {
        [SerializeField] private float _maxLifeTime = 30f;
        [SerializeField] private float _currentLifeTime;
        [SerializeField] private bool _isShrinking;
        [SerializeField] private GameObject _grid;

        public EnclosureStatsSettings Settings => GetComponent<EnclosureStatsSettings>();

        public Stats.Stat ConstrictionRate { get; private set; }

        private Tilemap _tilemap;

        private void Awake() => BaseAwake(Settings);

        private void OnEnable() => BaseOnEnable();

        private void OnDisable() => BaseOnDisable();

        private void FixedUpdate() => BaseFixedUpdate();

        protected void BaseAwake(EnclosureStatsSettings settings)
        {
            Debug.Log($"{gameObject.name} Enclosure Awake");
            base.BaseAwake(settings);
            ConstrictionRate = StatFactory.GetStat(settings.constrictionRate);
            _tilemap = GetComponent<Tilemap>();
        }

        protected override void BaseOnEnable()
        {
            _currentLifeTime = 0f;
        }

        protected override void BaseOnDisable()
        {
            Size.ClearModifiersList();
        }

        protected virtual void BaseFixedUpdate()
        {
            if (Time.timeScale == 0f) return;
            if (!_isShrinking) return;
            if (ConstrictionRate == null) return;
            if (_currentLifeTime >= _maxLifeTime)
            {
                _tilemap.ClearAllTiles();
                StopShrinking();
                return;
            }
            var constrictionRate = ConstrictionRate.Value * -1;
            var mod = new StatModifier(OperationType.Addition, constrictionRate);
            Size.AddModifier(mod);
            _currentLifeTime += Time.fixedDeltaTime;
            _grid.transform.localScale = new Vector3(Size.Value, Size.Value, 1);
        }

        public void StartShrinking()
        {
            _isShrinking = true;
        }

        public void StopShrinking()
        {
            _isShrinking = false;
        }
    }
}