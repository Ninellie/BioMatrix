using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enclosure : Entity
{
    private const float MaxLifeTime = 30f;
    private float _currentLifeTime;
    public EnclosureStatsSettings Settings => GetComponent<EnclosureStatsSettings>();
    public Stat ConstrictionRate { get; private set; }
    private void Awake() => BaseAwake(Settings);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update()
    {
        //BaseUpdate();
    }

    private void FixedUpdate() => BaseFixedUpdate();
    protected void BaseAwake(EnclosureStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enclosure Awake");
        base.BaseAwake(settings);
        ConstrictionRate = new Stat(settings.constrictionRate);
        
    }
    protected virtual void BaseFixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (ConstrictionRate == null) return;
        if (_currentLifeTime >= MaxLifeTime)
        {
            gameObject.SetActive(false);
            return;
        }
        var constrictionRate = ConstrictionRate.Value * -1;
        var mod = new StatModifier(OperationType.Addition, constrictionRate);
        Size.AddModifier(mod);
        _currentLifeTime += Time.fixedDeltaTime;
    }
    protected override void BaseOnEnable()
    {
        base.BaseOnEnable();
        var playerPos = FindObjectOfType<Player>().gameObject.transform.position;
        gameObject.transform.position = playerPos;
        _currentLifeTime = 0f;
    }
    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        Size.ClearModifiersList();
    }
}