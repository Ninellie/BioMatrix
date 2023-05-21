using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

    protected StatFactory statFactory;
    protected SpriteRenderer spriteRenderer;

    public readonly List<IEffect> effects = new();
    public GameTimeScheduler GameTimeScheduler { get; private set; }

    public void AddEffect(IEffect effect)
    {
        effects.Add(effect);
        effect.Attach(this);
        effect.Subscribe(this);
        Debug.LogWarning($"Effect {effect.Name} added to player and subscribed");
    }

    public string AddEffect(IEffect effect, float time)
    {
        AddEffect(effect);
        return GameTimeScheduler.Schedule(() => RemoveEffect(effect), time);
    }

    public void RemoveEffect(IEffect effect)
    {
        effect.Unsubscribe(this);
        effect.Detach();
        effects.Remove(effect);
    }

    private void OnEnable() => BaseOnEnable();
    
    private void OnDisable() => BaseOnDisable();
    
    private void Update() => BaseUpdate();
    
    protected void BaseAwake(EntityStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Entity Awake");
        GameTimeScheduler = Camera.main.GetComponent<GameTimeScheduler>();
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
        Size.ValueChangedEvent += ChangeCurrentSize;
        LifePoints.EmptyEvent += Death;
    }
    
    protected virtual void BaseOnDisable()
    {
        Size.ValueChangedEvent -= ChangeCurrentSize;
        LifePoints.EmptyEvent -= Death;
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

    public void AddStatModifier(StatModifier statModifier, string statName)
    {
        var stat = (Stat)EventHelper.GetPropByName(this, statName);
        stat?.AddModifier(statModifier);
    }
    public void RemoveStatModifier(StatModifier statModifier, string statName)
    {
        var stat = (Stat)EventHelper.GetPropByName(this, statName);
        stat?.RemoveModifier(statModifier);
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