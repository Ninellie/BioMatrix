using System;
using UnityEngine;

public class Unit : Entity
{
    public Action onDeath;
    public Stat Speed { get; private set; }
    public Stat AccelerationSpeed { get; private set; }
    public Stat RotationSpeed { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }

    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    protected virtual void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Unit Awake");
        base.BaseAwake(settings);
        Rb2D = GetComponent<Rigidbody2D>();

        Speed = statFactory.GetStat(settings.speed);
        AccelerationSpeed = statFactory.GetStat(settings.accelerationSpeed);
        RotationSpeed = statFactory.GetStat(settings.rotationSpeed);
    }
    protected override void Death()
    {
        onDeath?.Invoke();
        base.Death();
    }
}