using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Resource
{
    public Action onValueChanged;
    public Action onIncrease;
    public Action onDecrease;
    public Action onIncrement;
    public Action onDecrement;
    public Action onFill;
    public Action onRecover; 
    public Action onRecoveryStart;
    public Action onFullRecovery;
    public Action onEmpty;
    public Action onEdge;
    public Action onNotOnEdge;
    public Action onNotEmpty;


    public bool IsFull => Value == (int)_maxValueStat.Value;
    public bool IsEmpty => Value == _minValue;
    public bool IsOnEdge => Value == _minValue;
    public bool IsFullyRecovered => Value >= (int)_maxRecoverableValueStat.Value && _isRecovering;
    public bool IsOverRecovered => Value > (int)_maxRecoverableValueStat.Value && _isRecovering;
    public bool IsOnRecovery => Value < (int)_maxRecoverableValueStat.Value && _isRecovering;


    private readonly int _minValue;
    private readonly int _edgeValue;
    private readonly Stat _maxValueStat;
    private readonly Stat _maxRecoverableValueStat;
    private readonly Stat _recoverySpeedPerSecondStat;
    private readonly float _restoreRate;
    private readonly bool _isRecovering;
    private readonly bool _isLimited;


    public float TimeToRecover // Сюда записывается время до след. RestoreStep, просто добавь сюда Time.deltatime в апдейте
    {
        get => _timeToRecover;
        set
        {
            if (!_isRecovering)
            {
                return;
            }
            if (Value >= _maxRecoverableValueStat.Value)
            {
                return;
            }
            if (value > _restoreRate)
            {
                _timeToRecover = value % _restoreRate;
                ReserveValue += _recoverySpeedPerSecondStat.Value / _restoreRate;
            }
            else
            {
                _timeToRecover = value;
            }
        }
    }
    private float _timeToRecover;

    private float ReserveValue
    {
        get => _reserveValue;
        set
        {
            if (!_isRecovering)
            {
                return;
            }
            if (Value >= _maxRecoverableValueStat.Value)
            {
                return;
            }
            if (value > 1)
            {
                var intValue = (int)value;
                _reserveValue = value % 1;
                Value += intValue;
                onRecover?.Invoke();
            }
            else
            {
                _reserveValue = value;
            }
        }
    }
    private float _reserveValue;

    private int Value
    {
        get => _value;
        set
        {
            var dif = value - _value;
            if (dif == 0)
            {
                return;
            }

            var oldValue = _value;

            if (_isLimited)
            {
                if (value >= (int)_maxValueStat.Value)
                {
                    _value = (int)_maxValueStat.Value;
                    onFill?.Invoke();
                }
            }
            if (value <= _minValue)
            {
                _value = _minValue;
                onEmpty?.Invoke();
            }
            else
            {
                _value = value;
            }

            onValueChanged?.Invoke();

            if (_isRecovering)
            {
                if (oldValue < (int)_maxRecoverableValueStat.Value && IsFullyRecovered)
                {
                    onFullRecovery?.Invoke();
                }
                if (oldValue >= (int)_maxRecoverableValueStat.Value && IsOnRecovery)
                {
                    onRecoveryStart?.Invoke();
                }
            }

            if (dif > 0)
            {
                onIncrease?.Invoke();
                while (dif != 0)
                {
                    onIncrement?.Invoke();
                    dif--;
                }
            }
            if (dif < 0)
            {
                onDecrease?.Invoke();
                while (dif != 0)
                {
                    onDecrement?.Invoke();
                    dif++;
                }
            }

            if (oldValue == _minValue && !IsEmpty)
            {
                onNotEmpty?.Invoke();
            }
            if (oldValue == _edgeValue && _value > _edgeValue)
            {
                onNotOnEdge?.Invoke();
            }
            if (IsOnEdge)
            {
                onEdge?.Invoke();
            }
        }
    }
    private int _value;

    public Resource(int minValue) : this(minValue, minValue + 1, null, null, null, false, false)
    {
    }
    public Resource(int minValue, Stat maxValueStat) : this(minValue, minValue + 1, maxValueStat, null, null, false, true)
    {
    }
    public Resource(int minValue, Stat maxValueStat, Stat recoverySpeedPerSecondStat) : this(minValue, minValue + 1, maxValueStat, recoverySpeedPerSecondStat, maxValueStat, true, true)
    {
    }
    public Resource(int minValue, Stat maxValueStat, Stat recoverySpeedPerSecondStat, Stat maxRestorableValueStat) : this(minValue, minValue + 1, maxValueStat, recoverySpeedPerSecondStat, maxRestorableValueStat, true, true)
    {
    }
    public Resource(int minValue, int edgeValue, Stat maxValueStat, Stat recoverySpeedPerSecondStat, Stat maxRecoverableValueStat) : this(minValue, edgeValue, maxValueStat, recoverySpeedPerSecondStat, maxRecoverableValueStat, true, true)
    {
    }
    public Resource(int minValue, int edgeValue, Stat maxValueStat, Stat recoverySpeedPerSecondStat, Stat maxRecoverableValueStat, bool isRecovering, bool isLimited)
    {
        this._minValue = minValue;
        this._edgeValue = edgeValue;
        this._maxValueStat = maxValueStat;
        this._recoverySpeedPerSecondStat = recoverySpeedPerSecondStat;
        this._isRecovering = isRecovering;
        _restoreRate = 0.1f;
        _isLimited = isLimited;
        _maxRecoverableValueStat = maxRecoverableValueStat;
    }
    public void Set(int value)
    {
        Value = value;
    }
    public void Fill()
    {
        Value = (int)_maxValueStat.Value;
    }
    public void Empty()
    {
        Value = _minValue;
    }
    public void Increase(int value)
    {
        Value += value;
    }
    public void Increase()
    {
        Value++;
    }
    public void Decrease(int value)
    {
        Value -= value;
    }
    public void Decrease()
    {
        Value++;
    }
    public int GetMinValue()
    {
        return _minValue;
    }
    public int GetMaxValue()
    {
        return (int)_maxValueStat.Value;
    }
    public int GetLackValue()
    {
        return (int)(_maxValueStat.Value - Value);
    }
    public int GetValue()
    {
        return Value;
    }
    public float GetPercentValue()
    {
        var percent = _maxValueStat.Value / 100;
        var currentPercent = Value / percent;
        return currentPercent;
    }
}

public class Entity : MonoBehaviour
{
    public bool IsOnScreen { get; private set; }
    public bool Alive => LifePoints.IsEmpty;

    public const int DeathLifePointsThreshold = 0;

    public const int MinimalDamageTaken = 1;
    public Resource LifePoints { get; private set; }
    public Stat Size { get; private set; }
    public Stat MaximumLifePoints { get; private set; }
    public Stat LifeRegenerationPerSecond { get; private set; }
    public Stat KnockbackPower { get; private set; }
    public Stat Damage { get; private set; }
    public Stat GetStatByName(string statName)
    {
        return (Stat)GetType().GetProperty(statName).GetValue(this, null);
/*
        return statName switch
        {
            nameof(Size) => Size,
            nameof(MaximumLifePoints) => MaximumLifePoints,
            nameof(LifeRegenerationPerSecond) => LifeRegenerationPerSecond,
            nameof(KnockbackPower) => KnockbackPower,
            nameof(Damage) => Damage,
            _ => null
        };
*/
    }

    public Resource GetResourceByName(string resourceName)
    {
        return (Resource)GetType().GetProperty(resourceName).GetValue(this, null);
    }

    public Action GetActionByName(string actionName)
    {
        return (Action)GetType().GetField(actionName).GetValue(this);
/*
        return actionName switch
        {
            nameof(onCurrentLifePointsChanged) => onCurrentLifePointsChanged,
            nameof(onLifePointLost) => onLifePointLost,
            nameof(onLifePointRestore) => onLifePointRestore,
            _ => null
        };
*/
    }
    protected StatFactory statFactory;
    protected SpriteRenderer spriteRenderer;

    //private void Awake() => BaseAwake(GlobalStatsSettingsRepository.EntityStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    protected void BaseAwake(EntityStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Entity Awake");

        TryGetComponent<SpriteRenderer>(out SpriteRenderer sR);
        spriteRenderer = sR;

        Size = statFactory.GetStat(settings.size);
        MaximumLifePoints = statFactory.GetStat(settings.maximumLife);
        LifeRegenerationPerSecond = statFactory.GetStat(settings.lifeRegenerationInSecond);
        KnockbackPower = statFactory.GetStat(settings.knockbackPower);
        Damage = statFactory.GetStat(settings.damage);

        this.transform.localScale = new Vector3(Size.Value, Size.Value, 1);
        LifePoints = new Resource(DeathLifePointsThreshold, MaximumLifePoints, LifeRegenerationPerSecond);
        LifePoints.Fill();
    }
    protected virtual void BaseOnEnable()
    {
        Size.onValueChanged += ChangeCurrentSize;
    }
    protected virtual void BaseOnDisable()
    {
        Size.onValueChanged -= ChangeCurrentSize;
    }
    protected virtual void BaseUpdate()
    {
        if (Time.timeScale == 0) return;
        LifePoints.TimeToRecover += Time.deltaTime;
        if (spriteRenderer is null) return;
        IsOnScreen = CheckVisibilityOnCamera();
    }
    public virtual void TakeDamage(float amount)
    {
        LifePoints.Decrease((int)amount);
        Debug.Log("Damage is taken " + gameObject.name);
    }
    public virtual void RestoreLifePoints()
    {
        LifePoints.Fill();
    }
    protected virtual void Death()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    protected virtual void ChangeCurrentSize()
    {
        this.transform.localScale = new Vector3(Size.Value, Size.Value, 1);
    }
    private bool CheckVisibilityOnCamera()
    {
        var onScreen = spriteRenderer.isVisible;
        return onScreen;
    }
}