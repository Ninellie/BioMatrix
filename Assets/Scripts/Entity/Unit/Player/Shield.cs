using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity;
using Assets.Scripts.Entity.Stats;
using Assets.Scripts.Entity.Unit.Enemy;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private ShieldStatsSettings _settings;
    [SerializeField] private LayerMask _resistancePhysLayer;
    [SerializeField] private GameObject _shield;
    [SerializeField] private SpriteRenderer _shieldSprite;
    [SerializeField] private float _SpriteAlphaPerLayer = 0.2f;
    [SerializeField] private Color _shieldColor = Color.cyan;

    public Stat MaxLayers { get; private set; }
    public Stat MaxRechargeableLayers { get; private set; }
    public Stat RechargeRatePerSecond { get; private set; }
    public Stat RepulseForce { get; private set; }
    public Stat RepulseRadius { get; private set; }

    public RecoverableResource LayersCount { get; private set; }

    private CapsuleCollider2D _capsuleCollider;
    private StatFactory _statFactory;

    private void Awake()
    {
        _statFactory = Camera.main.GetComponent<StatFactory>();
        _shieldSprite = _shield.GetComponent<SpriteRenderer>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();

        LayersCount = new RecoverableResource(0, MaxLayers, RechargeRatePerSecond, MaxRechargeableLayers);

        MaxLayers = _statFactory.GetStat(_settings.maxLayers);

        MaxRechargeableLayers = _statFactory.GetStat(_settings.maxRechargeableLayers);
        var rechargeRatePerSecond = _settings.rechargeRate / 60f;
        RechargeRatePerSecond = _statFactory.GetStat(rechargeRatePerSecond);

        RepulseForce = _statFactory.GetStat(_settings.repulseForce);
        RepulseRadius = _statFactory.GetStat(_settings.repulseRadius);
    }

    private void OnEnable()
    {
        LayersCount.EmptyEvent += Enable;
        LayersCount.NotEmptyEvent += Disable;
        LayersCount.ValueChangedEvent += UpdateShieldAlpha;
        LayersCount.DecrementEvent += Repulse;
    }

    private void OnDisable()
    {
        LayersCount.EmptyEvent -= Enable;
        LayersCount.NotEmptyEvent -= Disable;
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

    private void Repulse()
    {
        var nearbyEnemies = GetNearbyEnemiesList(RepulseRadius.Value, _resistancePhysLayer);
        foreach (var enemy in nearbyEnemies)
            enemy.enemyMoveController.KnockBackFromTarget(RepulseForce.Value);

        //TODO Lead to the next target.GetComponent<MovementController>().KnockBackFromTarget(force);

        LayersCount.Decrease();
    }

    private List<Enemy> GetNearbyEnemiesList(float repulseRadius, LayerMask enemyLayer)
    {
        var colliders2D = Physics2D.OverlapCircleAll(transform.position, repulseRadius, enemyLayer);
        return colliders2D.Select(collider2d => collider2d.gameObject.GetComponent<Enemy>()).ToList();
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
        var spriteAlpha = _SpriteAlphaPerLayer * LayersCount.GetValue();
        _shieldColor.a = spriteAlpha;
        _shieldSprite.color = _shieldColor;
    }
}