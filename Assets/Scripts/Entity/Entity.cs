using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Entity : MonoBehaviour
{
    //public EntityStatsSettings Settings { get; set; }
    public Action onCurrentLifePointsChanged;
    public Action onLifePointLost;
    public Action onLifePointRestore;
    public bool IsOnScreen { get; private set; }
    public bool Alive => IsAlive();
    public const int DeathLifePointsThreshold = 0;
    public const int MinimalDamageTaken = 1;
    public const int LifePointAmount = 1;
    public float CurrentLifePoints
    {
        get => _currentLifePoints;
        protected set
        {
            Debug.Log($"Try to set life of {gameObject.name} to value: {value}");
            var dif = (int)value - (int)_currentLifePoints;
            if (dif > 0)
            {
                while (dif != 0)
                {
                    onLifePointRestore?.Invoke();
                    dif--;
                }
            }
            if (dif < 0)
            {
                while (dif != 0)
                {
                    onLifePointLost?.Invoke();
                    dif++;
                }
            }

            var difValue = value - DeathLifePointsThreshold;
        
            switch (difValue)
            {
                case < 0:
                    _currentLifePoints = DeathLifePointsThreshold;
                    Death();
                    break;
                case 0:
                    _currentLifePoints = value;
                    Death();
                    break;
                case > 0:
                    if (value >= MaximumLifePoints.Value)
                    {
                        _currentLifePoints = MaximumLifePoints.Value;
                        break;
                    }
                    _currentLifePoints = value;
                    break;
            }
            onCurrentLifePointsChanged?.Invoke();
        }
    }


    public Stat Size { get; private set; }
    public Stat MaximumLifePoints { get; private set; }
    public Stat LifeRegenerationPerSecond { get; private set; }
    public Stat KnockbackPower { get; private set; }
    public Stat Damage { get; private set; }

    //public object this[string propertyName]
    //{
    //    get => this.GetType().GetProperty(propertyName).GetValue(this, null);
    //    set => this.GetType().GetProperty(propertyName).SetValue(this, value, null);
    //}
    public Stat GetStatByName(string statName)
    {
        return (Stat)GetType().GetProperty(statName).GetValue(this, null);

        //return this[statName] as Stat;

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

    public Action GetActionByName(string actionName)
    {
        return (Action)GetType().GetProperty(actionName).GetValue(this, null);
        
        //return this[actionName] as Action;

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
    private float _currentLifePoints;
    private float _accumulatedLife = 0;

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

        _currentLifePoints = MaximumLifePoints.Value;
        Regeneration();
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
        if (spriteRenderer is null) return;
        IsOnScreen = CheckVisibilityOnCamera();
    }
    protected virtual void Regeneration()
    {
        if (gameObject == null)
        {
            return;
        }
        _accumulatedLife += LifeRegenerationPerSecond.Value;
        if (_accumulatedLife >= LifePointAmount)
        {
            _accumulatedLife -= LifePointAmount;
            RestoreLifePoints(LifePointAmount);
            Debug.Log($"Regeneration of {gameObject.name} completed");
        }
        Invoke("Regeneration", 1);
    }
    public virtual void TakeDamage(float amount)
    {
        CurrentLifePoints -= amount;
        Debug.Log("Damage is taken " + gameObject.name);
    }
    public virtual void RestoreLifePoints()
    {
        CurrentLifePoints = MaximumLifePoints.Value;
    }
    public virtual void RestoreLifePoints(int value)
    {
        if (value < LifePointAmount) return;

        if (value >= MaximumLifePoints.Value)
        {
            CurrentLifePoints = MaximumLifePoints.Value;
        }
        else
        {
            CurrentLifePoints += value;
        }
    }
    protected virtual bool IsAlive()
    {
        return CurrentLifePoints > DeathLifePointsThreshold;
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