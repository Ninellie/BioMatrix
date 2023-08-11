using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class EffectsRepositoryPreset : ScriptableObject
{
    [SerializeReference]
    private List<IEffect> _effects;

    public List<IEffect> GetEffects()
    {
        return _effects;
    }

    public IEffect GetEffectByName(string name)
    {
        return _effects.FirstOrDefault(e => e.Name.Equals(name));
    }

    [ContextMenu(nameof(AddNewStackableStatModifierEffect))]
    private void AddNewStackableStatModifierEffect()
    {
        var e = new StackableStatModifierEffect();
        _effects.Add(e);
    }

    [ContextMenu(nameof(AddNewStackableTemporaryStatModifierEffect))]
    private void AddNewStackableTemporaryStatModifierEffect()
    {
        var e = new StackableTemporaryStatModifierEffect();
        _effects.Add(e);
    }

    [ContextMenu(nameof(AddNewEffectAdderEffect))]
    private void AddNewEffectAdderEffect()
    {
        var e = new EffectAdderEffect();
        _effects.Add(e);
    }
}