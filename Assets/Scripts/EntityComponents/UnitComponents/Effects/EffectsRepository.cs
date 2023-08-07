using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Entity/Effects/EffectsRepository")]
public class EffectsRepository : MonoBehaviour
{
    [SerializeReference]
    private List<IEffect> _effects;

    [ContextMenu(nameof(AddNewStackableStatModifierEffect))]
    public void AddNewStackableStatModifierEffect()
    {
        var e = new StackableStatModifierEffect();
        _effects.Add(e);
    }

    [ContextMenu(nameof(AddNewStatModifierEffect))]
    public void AddNewStatModifierEffect()
    {
        var e = new StatModifierEffect();
        _effects.Add(e);
    }
}