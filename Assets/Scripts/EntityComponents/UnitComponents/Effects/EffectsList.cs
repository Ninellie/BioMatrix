using Assets.Scripts.GameSession.Events;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

[AddComponentMenu("Entity/Effects/EffectsList")]
public class EffectsList : MonoBehaviour
{
    //public IEffect this[string effectName] => GetEffectByName(effectName);

    [SerializeReference]
    private List<IEffect> _effects;

    [SerializeField]
    private GameTimeScheduler _gameTimeScheduler;

    private Player _player;
    private StatList _stats;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _stats = GetComponent<StatList>();
    }

    private void AdjustEffect(IEffect effect)
    {
        if (effect is IStackableEffect st)
            st.AddStack();

        if (effect is IStackableTemporaryEffect { IsStackSeparateDuration: true } stTe)
            _gameTimeScheduler.Schedule(() => RemoveEffectStack(stTe), stTe.Duration);

        if (effect is not ITemporaryEffect te) return;

        if (te.IsDurationStacks)
            _gameTimeScheduler.Prolong(te.Identifier, te.Duration);

        if (!te.IsDurationUpdates) return;
        var newTime = Time.timeSinceLevelLoad + te.Duration;
        _gameTimeScheduler.UpdateTime(te.Identifier, newTime);
    }

    private void AddNewEffect(IEffect effect)
    {
        _effects.Add(effect);

        if (effect is IStatModifier st)
            st.SetStatList(_stats);

        if (effect is IRespondingEffect re)
            re.Subscribe(_player);

        effect.Activate();

        if (effect is not ITemporaryEffect te) return;
        if (effect is IStackableTemporaryEffect { IsStackSeparateDuration: true } stTe)
            stTe.Identifier = _gameTimeScheduler.Schedule(() => RemoveEffectStack(stTe), te.Duration);

        te.Identifier = _gameTimeScheduler.Schedule(() => RemoveEffect(effect), te.Duration);
    }

    public void AddEffect(IEffect effect)
    {
        var first = _effects.FirstOrDefault(e => e.Name.Equals(effect.Name));
        if (first == null)
            AddNewEffect(effect);
        else
            AdjustEffect(first);
    }

    private void RemoveEffectStack(IStackableEffect effect)
    {
        effect.RemoveStack();
        if (effect.StacksCount == 0)
        {
            RemoveEffect(effect);
        }
    }

    private void RemoveEffect(IEffect effect)
    {
        effect.Deactivate();
        _effects.Remove(effect);
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