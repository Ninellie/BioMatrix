using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Enclosure : Entity
{
    private const float MaxLifeTime = 30f;
    private float _currentLifeTime;
    public Stat ConstrictionRate { get; private set; }
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.EnclosureStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => BaseFixedUpdate();
    protected void BaseAwake(EnclosureStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enclosure Awake");
        base.BaseAwake(settings);
        ConstrictionRate = new Stat(settings.ConstrictionRate);
    }
    protected virtual void BaseFixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (ConstrictionRate == null) return;
        if (_currentLifeTime >= MaxLifeTime)
        {
            this.gameObject.SetActive(false);
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
        var playerPos = FindObjectOfType<Player>().transform.position;
        this.transform.position = playerPos;
        _currentLifeTime = 0f;
    }
    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        Size.ClearModifiersList();
    }
}