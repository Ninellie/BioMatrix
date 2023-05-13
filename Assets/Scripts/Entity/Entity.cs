using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class EventHelper
{
    public static void AddActionByName(object target, string actionName, Action method)
    {
        var eventInfo = target.GetType().GetEvent(actionName);
        eventInfo.AddEventHandler(target, method);
    }
    public static void RemoveActionByName(object target, string actionName, Action method)
    {
        var eventInfo = target.GetType().GetEvent(actionName);
        eventInfo.RemoveEventHandler(target, method);
    }
    public static object GetPropByName(object target, string propName)
    {
        if (string.IsNullOrEmpty(propName))
        {
            return target;
        }

        var names = propName.Split('.');

        foreach (var name in names)
        {
            target = target.GetType().GetProperty(name).GetValue(target, null);
        }

        return target;
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
    }

    public Resource GetResourceByName(string resourceName)
    {
        return (Resource)GetType().GetProperty(resourceName).GetValue(this, null);
    }

    public Firearm GetFirearmByName(string propName)
    {
        return (Firearm)GetType().GetProperty(propName).GetValue(this, null);
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
        statFactory = Camera.main.GetComponent<StatFactory>();
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
        LifePoints.onEmpty += Death;
    }
    protected virtual void BaseOnDisable()
    {
        Size.onValueChanged -= ChangeCurrentSize;
        LifePoints.onEmpty -= Death;
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