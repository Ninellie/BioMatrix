using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Entity/Effects/EffectsList")]
public class EffectsList : MonoBehaviour
{
    //public IEffect this[string effectName] => GetEffectByName(effectName);

    [SerializeReference]
    private List<IEffect> _effects;

    public void AddEffect(IEffect effect)
    {

    }

    [ContextMenu(nameof(Activate))]
    public void Activate()
    {
        foreach (var effect in _effects)
        {
            effect.Activate();
        }
    }

    [ContextMenu(nameof(Deactivate))]
    public void Deactivate()
    {
        foreach (var effect in _effects)
        {
            effect.Deactivate();
        }
    }

    private IEffect GetEffectByName(string effectName)
    {
        foreach (var t in _effects.Where(t => t.Name == effectName))
        {
            return t;
        }
        Debug.LogError($"effect name: {effectName} not found in the list");
        return null;
    }
}