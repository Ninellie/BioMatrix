using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Effects
{
    [Serializable]
    public class ResourceIncreaserEffect : Effect, IResourceOperator, IStackableEffect
    {
        [Header("Incremental resource settings")]
        [SerializeField] private ResourceName _resourceName;
        [SerializeField, Min(0)] private int _value;

        [Header("Stacks settings")]
        [SerializeField] private int _maxStacks;
        [SerializeField] private int _currentStackCount;
        [SerializeField] private int _initialStacks;

        public int StacksCount => _currentStackCount;
        public int MaxStacks => _maxStacks;
        public int InitialStacks => _initialStacks;

        private ResourceList _resources;

        public void AddStack()
        {
            if (_currentStackCount == _maxStacks) return;
            _resources.GetResource(_resourceName).Increase(_value);
            _currentStackCount++;
        }

        public void RemoveStack()
        {
            if (_currentStackCount == 0) return;
            _resources.GetResource(_resourceName).Decrease(_value);
            _currentStackCount--;
        }

        public new void Activate()
        {
            while (_currentStackCount != _initialStacks)
            {
                AddStack();
            }
        }

        public new void Deactivate()
        {
            while (_currentStackCount > 0)
            {
                RemoveStack();
            }
        }
        public void SetResourceList(ResourceList resourceList)
        {
            _resources = resourceList;
        }
    }


    [Serializable]
    public class RespondingEffectAdderEffect : Effect, IResourceOperator, IEffectAdder, IRespondingEffect
    {
        [SerializeField] private List<string> _effectNames;
        [SerializeField] private ResourceName _resourceName;
        [SerializeField] private ResourceEventType _event;
        [SerializeField] private EffectsRepositoryPreset _effectPreset;

        private OverUnitDataAggregator _effectsAggregator;
        private Resource _resource;

        public void SetResourceList(ResourceList resourceList)
        {
            _resource = resourceList.GetResource(_resourceName);
        }

        public void SetEffectsManager(OverUnitDataAggregator effectsAggregator)
        {
            _effectsAggregator = effectsAggregator;
        }

        public void Subscribe()
        {
            _resource.AddListenerToEvent(_event, AddEffect);
        }

        private void AddEffect()
        {
            foreach (var effectName in _effectNames)
            {
                _effectsAggregator.AddEffect(_effectPreset.GetEffectByName(effectName));
            }
        }
    }

    [Serializable]
    public class StackableTemporaryStatModifierEffect : StackableStatModifierEffect, ITemporaryEffect
    {
        [SerializeField] private float _duration;
        [SerializeField] private bool _isDurationUpdates;
        [SerializeField] private bool _isDurationStacks;

        public float Duration => _duration;
        public bool IsDurationUpdates => _isDurationUpdates;
        public bool IsDurationStacks => _isDurationStacks;
        public string Identifier { get; set; }
    }

    [Serializable]
    public class StackableStatModifierEffect : StatModifierEffect, IStackableEffect
    {
        [SerializeField] private int _maxStacks;
        [SerializeField] private int _currentStackCount;
        [SerializeField] private int _initialStacks;

        public int StacksCount => _currentStackCount;
        public int MaxStacks => _maxStacks;
        public int InitialStacks => _initialStacks;

        public void AddStack()
        {
            if (StacksCount == MaxStacks)
                return;

            _currentStackCount++;
            AddMods();
        }

        public void RemoveStack()
        {
            if (StacksCount == 0)
                return;

            _currentStackCount--;
            RemoveMods();
        }

        public new void Activate()
        {
            while (_currentStackCount != _initialStacks)
            {
                AddStack();
            }
        }

        public new void Deactivate()
        {
            while (_currentStackCount > 0)
            {
                RemoveStack();
            }
        }
    }

    [Serializable]
    public class StatModifierEffect : Effect, IStatModifier
    {
        [SerializeField]
        protected List<StatModData> statModList;

        [SerializeField]
        protected StatList statList;
        public List<StatModData> StatModList => statModList;

        public new void Activate()
        {
            foreach (var statModData in statModList)
            {
                statList.GetStat(statModData.statName).AddModifier(statModData.mod);
            }
        }

        public new void Deactivate()
        {
            foreach (var statModData in statModList)
            {
                statList.GetStat(statModData.statName).RemoveModifier(statModData.mod);
            }
        }

        public void SetStatList(StatList statList)
        {
            this.statList = statList;
        }

        protected void AddMods()
        {
            foreach (var statModData in statModList)
            {
                statList.GetStat(statModData.statName).AddModifier(statModData.mod);
            }
        }

        protected void RemoveMods()
        {
            foreach (var statModData in statModList)
            {
                statList.GetStat(statModData.statName).RemoveModifier(statModData.mod);
            }
        }
    }
}