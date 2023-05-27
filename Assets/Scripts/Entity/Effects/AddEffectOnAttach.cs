﻿using System.Collections.Generic;

public class AddEffectOnAttach : IEffect
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TargetName { get; set; }
    public string Identifier { get; set; }

    public List<(IEffect effect, int stackCount)> Effects { get; set; }

    public bool IsTemporal { get; set; }
    public Stat Duration { get; set; }
    public bool IsDurationStacks { get; set; }
    public bool IsDurationUpdates { get; set; }

    public bool IsStacking { get; set; }
    public bool IsStackSeparateDuration { get; set; }
    public Stat MaxStackCount { get; set; }
    public Resource StacksCount { get; set; }

    private Entity _target;

    public void Attach(Entity target)
    {
        _target = target;
        AddEffects();
    }

    public void Detach()
    {
        while (StacksCount.IsEmpty)
        {
            RemoveEffects();
            StacksCount.Decrease();
        }
    }

    public void Subscribe(Entity target)
    {
        StacksCount.IncrementEvent += AddEffects;
        StacksCount.DecrementEvent += RemoveEffects;
    }

    public void Unsubscribe(Entity target)
    {
        StacksCount.IncrementEvent -= AddEffects;
        StacksCount.DecrementEvent -= RemoveEffects;
    }

    private void AddEffects()
    {
        foreach (var tuple in Effects)
        {
            for (int i = 0; i < tuple.stackCount; i++)
            {
                _target.AddEffectStack(tuple.effect);
            }
            
        }
    }

    private void RemoveEffects()
    {
        foreach (var tuple in Effects)
        {
            for (int i = 0; i < tuple.stackCount; i++)
            {
                _target.RemoveEffectStack(tuple.effect);
            }
        }
    }

    public AddEffectOnAttach(
        string name,
        string description,
        string targetName,
        List<(IEffect effect, int stackCount)> effects
    ) : this(
        name,
        description,
        targetName,
        effects,
        false,
        new Stat(0, false),
        false,
        false,
        false,
        false,
        new Stat(1, false)
    )
    {
    }

    public AddEffectOnAttach(
        string name,
        string description,
        string targetName,
        List<(IEffect effect, int stackCount)> effects,
        bool isTemporal,
        Stat duration,
        bool isDurationStacks,
        bool isDurationUpdates,
        bool isStacking,
        bool isStackSeparateDuration,
        Stat maxStackCount
    )
    {
        Name = name;
        Description = description;
        TargetName = targetName;
        Effects = effects;
        IsTemporal = isTemporal;
        Duration = duration;
        IsDurationStacks = isDurationStacks;
        IsDurationUpdates = isDurationUpdates;
        IsStacking = isStacking;
        IsStackSeparateDuration = isStackSeparateDuration;
        MaxStackCount = maxStackCount;
        StacksCount = new Resource(MaxStackCount);
    }
}