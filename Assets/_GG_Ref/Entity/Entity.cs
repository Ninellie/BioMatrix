using System;
using JetBrains.Annotations;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool IsOnScreen { get; protected set; }
    public bool Alive => IsAlive();
    public float CurrentLifePoints
    {
        get => CurrentLifePoints;
        protected set
        {
            var difValue = value - DeathLifePointsLevel;
            switch (difValue)
            {
                case < 0:
                    CurrentLifePoints = DeathLifePointsLevel;
                    Death();
                    break;
                case 0:
                    CurrentLifePoints = value;
                    Death();
                    break;
                case > 0:
                    CurrentLifePoints = value;
                    break;
            }
        }
    }
    protected virtual EntityStatsSettings Settings => GlobalStatsSettingsRepository.EntityStats;
    protected Stat size;
    protected Stat maximumLifePoints;
    private const int DeathLifePointsLevel = 0;
    public const int MinimalDamageTaken = 1;
    private Camera _mCamera;
    protected virtual void Awake()
    {
        _mCamera = FindObjectOfType<Camera>();
        SetStats(Settings);
    }
    protected virtual void OnEnable()
    {
        size.onValueChanged += ChangeCurrentSize;
    }
    protected virtual void OnDisable()
    {
        size.onValueChanged -= ChangeCurrentSize;
    }
    protected virtual void Update()
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
        return CurrentLifePoints > DeathLifePointsLevel;
    }
    protected virtual void Death()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    protected virtual void ChangeCurrentSize()
    {
        gameObject.GetComponent<Transform>().position.Scale(new Vector3(size.Value, size.Value));
    }
    protected virtual void SetStats([NotNull] EntityStatsSettings settings)
    {
        if (settings == null) throw new ArgumentNullException(nameof(settings));
        
        size = new Stat(settings.Size);
        maximumLifePoints = new Stat(settings.MaximumLife);
    }
}