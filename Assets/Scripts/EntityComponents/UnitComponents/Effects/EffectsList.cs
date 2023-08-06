using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Entity/Effects/EffectsList")]
public class EffectsList : MonoBehaviour
{
    public IEffect this[string effectName] => GetEffectByName(effectName);

    [SerializeField]
    private List<Effect> _effects;

    [SerializeField]
    private List<StatModifierEffect> _statModifierEffects;

    [Header("Add effect")]
    [Space]
    public StatModifierEffect effect;

    [ContextMenu(nameof(AddNewEffectToList))]
    public void AddNewEffectToList()
    {
        var e = effect;
        _effects.Add(e);
        effect = null;
    }

    [ContextMenu(nameof(AddNewStatEffectToList))]
    public void AddNewStatEffectToList()
    {
        var e = new StatModifierEffect();
        _effects.Add(e);
    }

    [ContextMenu(nameof(Activate))]
    public void Activate()
    {
        effect.Activate();
    }

    [ContextMenu(nameof(Deactivate))]
    public void Deactivate()
    {
        effect.Deactivate();
    }

    private IEffect GetEffectByName(string effectName)
    {
        foreach (var t in _effects.Where(t => t.Name == effectName))
        {
            return t;
        }
        Debug.LogError($"effect name: {effectName} not found in the list");
        throw new ArgumentException($"effect name: {effectName} not found in the list");
    }
}