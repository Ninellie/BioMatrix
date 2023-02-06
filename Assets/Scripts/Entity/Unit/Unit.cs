using System;
using UnityEngine;

public class Unit : Entity
{
    public Action onDeath;
    protected Stat Speed { get; private set; }
    protected Stat TurningSpeed { get; private set; }
    protected Movement Movement { get; private set; }
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.UnitStats);
    private void Start() => BaseStart();
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
    protected void BaseAwake(UnitStatsSettings settings, Movement movement = null)
    {
        Debug.Log($"{gameObject.name} Unit Awake");
        base.BaseAwake(settings);
        Speed = new Stat(settings.Speed);
        Movement = movement ?? new Movement(this, Speed.Value);
    }
    protected void BaseStart()
    {
        Movement.SetVelocity();
    }
    protected override void BaseOnEnable()
    {
        base.BaseOnEnable();
        if (Speed != null) Speed.onValueChanged += ChangeCurrentSpeed;
    }
    protected override void BaseOnDisable()
    {
        base.BaseOnDisable();
        if (Speed != null) Speed.onValueChanged -= ChangeCurrentSpeed;
    }
    protected void KnockBack(Entity collisionEntity)
    {
        Movement.KnockBack(collisionEntity);
    }
    protected override void Death()
    {
        onDeath?.Invoke();
        base.Death();
    }
    protected void ChangeCurrentSpeed()
    {
        var oldSpeed = Movement.Speed;
        var speedDif = Speed.Value - oldSpeed;
        switch (speedDif)
        {
            case > 0:
                Movement.Accelerate(speedDif);
                break;
            case < 0:
                Movement.SlowDown(speedDif * -1);
                break;
            case 0:
                throw new ArgumentOutOfRangeException();
        }
    }
}