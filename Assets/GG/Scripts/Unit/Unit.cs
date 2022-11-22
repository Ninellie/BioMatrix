using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatModifier
{
    public StatModifier(OperationType type, float value)
    {
        Type = type;
        Value = value;
    }
    public OperationType Type { get; }
    public float Value { get; }
}
public class Stat
{
    public float Value => GetActualValue();
    public IEnumerable<StatModifier> Modifiers => _modifiers;
    public Action onValueChanged;
    private readonly float _baseValue;
    private readonly bool _isModifiable;
    private readonly float _baseAddedValue;
    private float AddedValue => GetAddedValue();
    private readonly float _baseMultiplierValue;
    private float MultiplierValue => GetMultiplierValue();
    private readonly List<StatModifier> _modifiers;
    private float _oldValue;
    public Stat(float baseValue) : this(baseValue, true)
    {
        _modifiers = new List<StatModifier>();
    }
    //Without multiplierValue (with multiplierValue = 1)
    public Stat(float baseValue, bool isModifiable) : this(baseValue, isModifiable, 1)
    {
    }
    public Stat(float baseValue, List<StatModifier> modifiers) : this(baseValue, 1, modifiers)
    {
    }
    //Without addedValue (with addedValue = 0)
    public Stat(float baseValue, float baseMultiplierValue, List<StatModifier> modifiers) : this(baseValue, baseMultiplierValue, 0, modifiers)
    {
    }
    public Stat(float baseValue, bool isModifiable, float baseMultiplierValue) : this(baseValue, isModifiable, baseMultiplierValue, 0)
    {
    }
    //Base constructors
    public Stat(float baseValue, float baseMultiplierValue, float baseAddedValue, List<StatModifier> modifiers) : this(baseValue, true, baseMultiplierValue, baseAddedValue)
    {
        _modifiers = modifiers;
    }
    public Stat(float baseValue, bool isModifiable, float baseMultiplierValue, float baseAddedValue)
    {
        _baseValue = baseValue;
        _isModifiable = isModifiable;
        _baseMultiplierValue = baseMultiplierValue;
        _baseAddedValue = baseAddedValue;
    }
    public void AddModifier(StatModifier modifier)
    {
        RememberCurrentValue();
        _modifiers.Add(modifier);
        OnValueChanged();
    }
    public bool RemoveModifier(StatModifier modifier)
    {
        RememberCurrentValue();
        if (_modifiers.Contains(modifier))
        {
            _modifiers.Remove(modifier);
            OnValueChanged();
            return true;
        };
        return false;
    }
    public bool ClearModifiersList()
    {
        RememberCurrentValue();
        if (_modifiers.Count == 0)
        {
            return false;
        }
        _modifiers.Clear();
        OnValueChanged();
        return true;
    }
    private float GetAddedValue()
    {
        return _modifiers.Where(modifier => modifier.Type == OperationType.Addition).Sum(modifier => modifier.Value) + _baseAddedValue;
    }
    private float GetMultiplierValue()
    {
        return _modifiers.Where(modifier => modifier.Type == OperationType.Multiplication).Sum(modifier => modifier.Value) + _baseMultiplierValue;
    }
    private float GetActualValue()
    {
        if (_isModifiable == false)
        {
            return (_baseValue + _baseAddedValue) * _baseMultiplierValue;
        }
        return (_baseValue + AddedValue) * MultiplierValue;
    }
    private void RememberCurrentValue()
    {
        _oldValue = GetActualValue();
    }
    private void OnValueChanged()
    {
        if (Value.Equals(_oldValue))
        {
            onValueChanged?.Invoke();
        }
    }
}

public class Ammoo : Projectile
{
    public override void Launch(Vector2 direction, FirearmSettings firearmSettings)
    {
        Vector2 actualShotDirection = GetActualShotDirection(direction, firearmSettings.MaxShotDeflectionAngle);
        gameObject.AddComponent<Movement>();
        gameObject.GetComponent<Movement>().ChangeMode(MovementMode.Rectilinear);
        gameObject.GetComponent<Movement>().AccelerateInDirection(firearmSettings.ShootForce, actualShotDirection);
    }
    private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
    {
        float angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
        float shotDeflectionAngle = Lib2DMethods.Range(-angleInRad, angleInRad);
        return Lib2DMethods.Rotate(direction, shotDeflectionAngle);
    }
}
public class Projectile : Unit
{
    public void Launch(Vector2 direction, float force)
    {
        movement.ChangeMode(MovementMode.Rectilinear);
        movement.AccelerateInDirection(force, direction);
    }
}
//public class Enemy : Unit
//{
//    private int _damage;
//}
public class Playerr : Unit
{
    private int _level;
    private int _experience;
    private int expToLvlup;
    private Weapon _weapon;
    private SpriteRenderer Sprite => GetComponent<SpriteRenderer>();
    

    
    protected new void OnEnable()
    {
        base.OnEnable();
    }
    protected new void OnDisable()
    {
        base.OnDisable();
    }
    protected new void Awake()
    {
        base.Awake();
       
    }
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public void OnMove(InputValue input)
    {
        Vector2 inputVector2 = input.Get<Vector2>();
        movement.SetMovementDirection(inputVector2);
        switch (inputVector2.x)
        {
            case < 0:
                Sprite.flipX = true;
                break;
            case > 0:
                Sprite.flipX = false;
                break;
        }
    }
    public void OnFire()
    {
        _weapon.IsTriggerPulled = true;
    }
    public void OnFireOff()
    {
        _weapon.IsTriggerPulled = false;
    }
}

public class UnitStatsSettings : EntityStatsSettings
{
    public float Speed { get; set; }
}
public class EntityStatsSettings
{
    public float Size { get; set; }
    public float MaximumLife { get; set; }
}
public class FireArmStatsSettings
{
    public float Damage { get; set; }
    public float ShootForce { get; set; }
    public float ShootsPerSecond { get; set; }
    public float MaxShootDeflectionAngle { get; set; }
    public float MagazineSize { get; set; }
    public float ReloadSpeed { get; set; }
    public float SingleShootProjectile { get; set; }
    
}
public class GlobalStatsSettingsRepository
{
    public static readonly EntityStatsSettings EntityStats = new EntityStatsSettings
    {
        Size = 1,
        MaximumLife = 1,
    };

    public static readonly UnitStatsSettings UnitStats = new UnitStatsSettings()
    {
        Size = 1,
        MaximumLife = 1,
        Speed = 100,
    };

    public static readonly FireArmStatsSettings ShotgunSettings = new FireArmStatsSettings()
    {
        Damage = 1,
        ShootForce = 700,
        ShootsPerSecond = 1,
        MaxShootDeflectionAngle = 15,
        MagazineSize = 2,
        ReloadSpeed = 1,
        SingleShootProjectile = 10,
    };
}
public class Unit : Entity
{
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
    protected void Awake()
    {
        SetStats();
        SetMovement();
        RestoreLifePoints();
    }
    protected void FixedUpdate()
    {
        movement.FixedUpdateMove();
    }
    protected virtual void SetStats()
    {
        speed = new Stat(GlobalStatsSettingsRepository.UnitStats.Speed);
        size = new Stat(GlobalStatsSettingsRepository.UnitStats.Size);
        maximumLifePoints = new Stat(GlobalStatsSettingsRepository.UnitStats.MaximumLife);
    }
    protected virtual void SetMovement()
    {
        movement = new Movement(gameObject, speed.Value);
    }
    protected virtual void RestoreLifePoints()
    {
        currentLifePoints = maximumLifePoints.Value;
    }
    private void ChangeCurrentSpeed()
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
public class Entity : MonoBehaviour
{
    protected Stat size = new Stat(1);
    protected Stat maximumLifePoints = new Stat(1);
    protected float currentLifePoints;

    protected void OnEnable()
    {
        size.onValueChanged += ChangeCurrentSize;
    }
    protected void OnDisable()
    {
        size.onValueChanged -= ChangeCurrentSize;
    }
    protected void ChangeCurrentSize()
    {
        gameObject.GetComponent<Transform>().position.Scale(new Vector3(size.Value, size.Value));
    }
}
public class Firearmr : MonoBehaviour
{
    private Stat _damage;
    private Stat _shootForce;
    private Stat _shootsPerSecond;
    private Stat _maxShootDeflectionAngle;
    private Stat _magazineSize;
    private Stat _reloadSpeed;
    private Stat _singleShootProjectile;
    private Ammoo _ammo;
    private readonly Shooter _shooter = new();
    private readonly Reload _reload = new();
    private readonly Magazine _magazine = new();

    private void Awake()
    {
        _magazine.size = _magazineSize.Value;

    }
    protected virtual void SetStats()
    {
        speed = new Stat(GlobalStatsSettingsRepository.UnitStats.Speed);
    }
}