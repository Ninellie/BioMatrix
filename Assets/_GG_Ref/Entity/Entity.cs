using System;
using JetBrains.Annotations;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Action onCurrentLifePointsChanged;
    public Action onEnableAction;
    public Action onDisableAction;
    public bool IsOnScreen { get; private set; }
    public bool Alive => IsAlive();
    public const int DeathLifePointsThreshold = 0;
    public const int MinimalDamageTaken = 1;
    public float CurrentLifePoints
    {
        get => _currentLifePoints;
        protected set
        {
            var difValue = value - DeathLifePointsThreshold;
            switch (difValue)
            {
                case < 0:
                    _currentLifePoints = DeathLifePointsThreshold;
                    onCurrentLifePointsChanged?.Invoke();
                    Death();
                    break;
                case 0:
                    _currentLifePoints = value;
                    onCurrentLifePointsChanged?.Invoke();
                    Death();
                    break;
                case > 0:
                    _currentLifePoints = value;
                    onCurrentLifePointsChanged?.Invoke();
                    break;
            }
        }
    }
    protected Stat Size { get; private set; }
    protected Stat MaximumLifePoints { get; private set; }
    private float _currentLifePoints;
    private Camera _mCamera;
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.EntityStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    protected void BaseAwake(EntityStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Entity Awake");
        _mCamera = FindObjectOfType<Camera>();
        Size = new Stat(settings.Size);
        MaximumLifePoints = new Stat(settings.MaximumLife);
        _currentLifePoints = MaximumLifePoints.Value;
    }
    protected virtual void BaseOnEnable()
    {
        onEnableAction?.Invoke();
        Size.onValueChanged += ChangeCurrentSize;
    }
    protected virtual void BaseOnDisable()
    {
        onDisableAction?.Invoke();
        Size.onValueChanged -= ChangeCurrentSize;
    }
    protected virtual void BaseUpdate()
    {
        IsOnScreen = Lib2DMethods.CheckVisibilityOnCamera(_mCamera, gameObject);
    }
    public virtual void TakeDamage(float amount)
    {
        var lifePoints = CurrentLifePoints - amount;
        CurrentLifePoints = lifePoints;
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
    protected virtual void RestoreLifePoints()
    {
        CurrentLifePoints = MaximumLifePoints.Value;
    }
    protected virtual void ChangeCurrentSize()
    {
        gameObject.GetComponent<Transform>().position.Scale(new Vector3(Size.Value, Size.Value));
    }
}