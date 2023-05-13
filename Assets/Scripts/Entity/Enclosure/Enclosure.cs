using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class Enclosure : Entity
{
    public EnclosureStatsSettings Settings => GetComponent<EnclosureStatsSettings>();
    public Stat ConstrictionRate { get; private set; }
    [SerializeField] private const float MaxLifeTime = 30f;
    [SerializeField] private float _currentLifeTime;
    [SerializeField] private bool _isShrinking;
    [SerializeField] private GameObject _grid;
    private Tilemap _tilemap;

    private void Awake() => BaseAwake(Settings);

    private void OnEnable() => BaseOnEnable();

    private void OnDisable() => BaseOnDisable();

    private void FixedUpdate() => BaseFixedUpdate();

    protected void BaseAwake(EnclosureStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enclosure Awake");
        base.BaseAwake(settings);
        ConstrictionRate = statFactory.GetStat(settings.constrictionRate);
        _tilemap = GetComponent<Tilemap>();
    }

    protected virtual void BaseFixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (!_isShrinking) return;
        if (ConstrictionRate == null) return;
        if (_currentLifeTime >= MaxLifeTime)
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

    protected override void BaseOnEnable()
    {
        _currentLifeTime = 0f;
    }

    protected override void BaseOnDisable()
    {
        Size.ClearModifiersList();
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