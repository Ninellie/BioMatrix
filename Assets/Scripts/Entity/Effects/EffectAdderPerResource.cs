﻿using Assets.Scripts.Core;

namespace Assets.Scripts.Entity.Effects
{
    public class EffectAdderPerResource : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }

        public IEffect Effect { get; set; }
        public PropTrigger TriggerResource { get; set; }

        public string Identifier { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.Stat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stats.Stat MaxStackCount { get; set; }

        private Resource _triggerResource;
        private int _value;
        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            _triggerResource = (Resource)EventHelper.GetPropByPath(target, TriggerResource.Path);
            _value = _triggerResource.GetValue();
            UpdateMods();
        }

        public void Detach()
        {
            while (_value > 0)
            {
                _target.RemoveEffectStack(Effect);
                _value--;
            }
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, TriggerResource.Path),
                TriggerResource.Name, UpdateMods);
            StacksCount.IncrementEvent += AddStack;
            StacksCount.DecrementEvent += RemoveStack;
        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, TriggerResource.Path),
                TriggerResource.Name, UpdateMods);
            StacksCount.IncrementEvent -= AddStack;
            StacksCount.DecrementEvent -= RemoveStack;
        }

        private void AddStack()
        {
            for (int i = 0; i < _value; i++)
            {
                _target.AddEffectStack(Effect);
            }
        }

        private void RemoveStack()
        {
            for (int i = 0; i < _value; i++)
            {
                _target.RemoveEffectStack(Effect);
            }
        }

        private void UpdateMods()
        {
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                var dif = _triggerResource.GetValue() - _value;
                while (dif != 0)
                {
                    switch (dif)
                    {
                        case > 0:
                            _target.AddEffectStack(Effect);
                            dif--;
                            break;
                        case < 0:
                            _target.RemoveEffectStack(Effect);
                            dif++;
                            break;
                    }
                }
            }

            _value = _triggerResource.GetValue();
        }

        public EffectAdderPerResource(
            string name,
            string description,
            string targetName,
            IEffect effect,
            PropTrigger triggerResource
        ) : this(
            name,
            description,
            targetName,
            effect,
            triggerResource,
            false,
            new Stats.Stat(0, false),
            false,
            false,
            false,
            false,
            new Stats.Stat(1, false)
        )
        {
        }

        public EffectAdderPerResource(
            string name,
            string description,
            string targetName,
            IEffect effect,
            PropTrigger triggerResource,
            bool isTemporal,
            Stats.Stat duration,
            bool isDurationStacks,
            bool isDurationUpdates,
            bool isStacking,
            bool isStackSeparateDuration,
            Stats.Stat maxStackCount
        )
        {
            Name = name;
            Description = description;
            TargetName = targetName;
            Effect = effect;
            TriggerResource = triggerResource;
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
}