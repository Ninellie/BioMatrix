using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Action onCurrentLifePointsChanged;
    public bool IsOnScreen { get; private set; }
    public bool Alive => IsAlive();
    public const int DeathLifePointsThreshold = 0;
    public const int MinimalDamageTaken = 1;
    public float CurrentLifePoints
    {
        get => _currentLifePoints;
        protected set
        {
            Debug.Log($"Try to set life of {gameObject.name} to value: {value}");
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
                    _currentLifePoints = value;
                    break;
            }
            onCurrentLifePointsChanged?.Invoke();
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
        //Size.onValueChanged += ChangeCurrentSize;
    }
    protected virtual void BaseOnDisable()
    {
        //Size.onValueChanged -= ChangeCurrentSize;
    }
    protected virtual void BaseUpdate()
    {
        IsOnScreen = CheckVisibilityOnCamera(_mCamera, gameObject);
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
        if (value < 1) return;
        CurrentLifePoints += value;
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
        //gameObject.GetComponent<Transform>().position.Scale(new Vector3(Size.Value, Size.Value));
    }
    private bool CheckVisibilityOnCamera(Camera camera, GameObject gameObject)
    {
        var screenPos = camera.WorldToScreenPoint(gameObject.transform.position);
        var onScreen = screenPos.x > 0f && 
                       screenPos.x < Screen.width &&
                       screenPos.y > 0f &&
                       screenPos.y < Screen.height;
        return onScreen;
    }
}