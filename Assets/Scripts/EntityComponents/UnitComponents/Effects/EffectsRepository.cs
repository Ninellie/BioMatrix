using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Entity/Effects/EffectsRepository")]
public class EffectsRepository : MonoBehaviour
{
    [SerializeField]
    private bool _usePreset;

    [SerializeField]
    private List<EffectsRepositoryPreset> _presets;

    [SerializeReference]
    private List<IEffect> _defaultEffects;

    [SerializeReference]
    private List<IEffect> _effects;
    

    private void Awake()
    {
        if (_usePreset)
            foreach (var preset in _presets)
            {
                var effectList = new List<IEffect>();
                effectList = preset.GetEffects();
                _effects.AddRange(effectList);
            }
        else
            _effects = _defaultEffects;
    }

    public IEffect GetEffectByName(string name)
    {
        return _effects.FirstOrDefault(e => e.Name.Equals(name));
    }
    public string GetEffectDescriptionByName(string name)
    {
        return _effects.FirstOrDefault(e => e.Name.Equals(name)).Description;
    }

    [ContextMenu(nameof(AddNewStackableStatModifierEffect))]
    private void AddNewStackableStatModifierEffect()
    {
        var e = new StackableStatModifierEffect();
        _defaultEffects.Add(e);
    }

    [ContextMenu(nameof(AddNewStackableTemporaryStatModifierEffect))]
    private void AddNewStackableTemporaryStatModifierEffect()
    {
        var e = new StackableTemporaryStatModifierEffect();
        _defaultEffects.Add(e);
    }

    [ContextMenu(nameof(AddNewEffectAdderEffect))]
    private void AddNewEffectAdderEffect()
    {
        var e = new RespondingEffectAdderEffect();
        _defaultEffects.Add(e);
    }
}