using System;
using JetBrains.Annotations;


public class Unit : Entity
{
    protected override EntityStatsSettings Settings => GlobalStatsSettingsRepository.UnitStats;
    protected Stat speed;
    protected Movement movement;
    protected new void OnEnable()
    {
        base.OnEnable();
        speed.onValueChanged += ChangeCurrentSpeed;
    }
    protected new void OnDisable()
    {
        base.OnDisable();
        speed.onValueChanged -= ChangeCurrentSpeed;
    }
    protected new void Awake()
    {
        SetStats(Settings);
        SetMovement();
        RestoreLifePoints();
    }
    protected void FixedUpdate()
    {
        movement.FixedUpdateMove();
    }
    protected void SetStats([NotNull] UnitStatsSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        size = new Stat(settings.Size);

        speed = new Stat(settings.Speed);
        
        maximumLifePoints = new Stat(settings.MaximumLife);
    }
    protected virtual void SetMovement()
    {
        movement = new Movement(gameObject, speed.Value);
    }
    protected virtual void RestoreLifePoints()
    {
        currentLifePoints = maximumLifePoints.Value;
    }
    protected void ChangeCurrentSpeed()
    {
        var oldSpeed = movement.Speed;
        var speedDif = speed.Value - oldSpeed;
        switch (speedDif)
        {
            case > 0:
                movement.Accelerate(speedDif);
                break;
            case < 0:
                movement.SlowDown(speedDif * -1);
                break;
            case 0:
                throw new ArgumentOutOfRangeException();
        }
    }
}