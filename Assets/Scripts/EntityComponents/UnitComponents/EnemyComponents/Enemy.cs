using Assets.Scripts.Core.Render;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.SourceStatSystem;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        [Space]
        [Header("Stats settings")]
        private StatSourcesComponent _statSources;
        private StatListComponent _statList;

        public bool IsAlive { get; private set; }

        public EnemyType EnemyType => _enemyType;

        private readonly Rarity _rarity = new();

        private Rigidbody2D _rigidbody2D;
        private SpriteOutline _spriteOutline;
        private ResourceList _resources;
        private bool _isSubscribed;


        private void Awake()
        {
            Debug.Log($"Enemy {gameObject.name} Awake");
            IsAlive = true;
            _resources = GetComponent<ResourceList>();
            _spriteOutline = GetComponent<SpriteOutline>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            if (_statSources == null) _statSources = GetComponent<StatSourcesComponent>();
            if (_statList == null) _statList = GetComponent<StatListComponent>();
            _rarity.Value = RarityEnum.Normal;
        }

        private void Start()
        {
            Subscribe();
            UpdateCurrentSize();
        }

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        //todo to rotationController?
        public void LookAt2D(Vector2 targetPosition)
        {
            var direction = (Vector2)_rigidbody2D.transform.position - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            _rigidbody2D.rotation = angle;
            _rigidbody2D.SetRotation(angle);
        }
        //todo to rotationController?
        public void TakeAsTarget()
        {
            var color = Color.red;
            _spriteOutline.enabled = true;
            _spriteOutline.color = color;
        }

        public void RemoveFromTarget()
        {
            _spriteOutline.color = _rarity.Color;
            _spriteOutline.enabled = _rarity.Value != RarityEnum.Normal;
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
            //_stats.GetStat(StatName.MaximumHealth).AddModifier(statMod);

            //var s = new StatSourceData()
            //_statSources.AddStatSource();

            _resources.GetResource(ResourceName.Health).Fill();
        }

        private void Subscribe()
        {
            if (_isSubscribed) return;
            //var sizeStat = _stats.GetStat(StatName.Size);
            //if (sizeStat is null) return;
            //_stats.GetStat(StatName.Size).valueChangedEvent.AddListener(UpdateCurrentSize);
            _isSubscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_isSubscribed) return;
            //_stats.GetStat(StatName.Size).valueChangedEvent.RemoveListener(UpdateCurrentSize);
            _isSubscribed = false;
        }

        private void UpdateCurrentSize()
        {
            //var sizeValue = _stats.GetStat(StatName.Size).Value;
            //transform.localScale = new Vector3(sizeValue, sizeValue, 1);
        }
    }
}