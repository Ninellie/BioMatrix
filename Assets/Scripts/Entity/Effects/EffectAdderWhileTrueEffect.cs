using Assets.Scripts.Core;
using Assets.Scripts.Entity.Stats;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Entity.Effects
{
    public class EffectAdderWhileTrueEffect : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }

        public PropTrigger AddTrigger { get; set; }
        public PropTrigger RemoveTrigger { get; set; }
        public string ResourceConditionPath { get; set; }

        public List<(IEffect effect, int stackCount)> Effects { get; set; }

        public string Identifier { get; set; }

        public bool IsTemporal { get; set; }
        public Stat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stat MaxStackCount { get; set; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            var isAddCondition = (bool)EventHelper.GetPropByPath(target, ResourceConditionPath);
            if (isAddCondition)
            {
                AddEffects();
            }
        }

        public void Detach()
        {
            RemoveEffects();
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, AddTrigger.Path), AddTrigger.Name, AddEffects);
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveEffects);
            StacksCount.IncrementEvent += TryAddEffectsStack;
            StacksCount.DecrementEvent += TryRemoveEffectsStack;

        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveEffects);
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, AddTrigger.Path), AddTrigger.Name, AddEffects);
            StacksCount.IncrementEvent -= TryAddEffectsStack;
            StacksCount.DecrementEvent -= TryRemoveEffectsStack;
        }

        private void TryAddEffectsStack()
        {
            var isAddCondition = (bool)EventHelper.GetPropByPath(_target, ResourceConditionPath);
            if (isAddCondition)
            {
                AddEffectsStack();
            }
        }

        private void TryRemoveEffectsStack()
        {
            var isAddCondition = (bool)EventHelper.GetPropByPath(_target, ResourceConditionPath);
            if (isAddCondition)
            {
                RemoveEffectsStack();
            }
        }

        private void AddEffectsStack()
        {
            foreach (var tuple in Effects)
            {
                for (int i = 0; i < tuple.stackCount; i++)
                {
                    _target.RemoveEffectStack(tuple.effect);
                }
            }
        }

        private void RemoveEffectsStack()
        {
            foreach (var tuple in Effects)
            {
                for (int i = 0; i < tuple.stackCount; i++)
                {
                    _target.AddEffectStack(tuple.effect);
                }
            }
        }

        private void AddEffects()
        {
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                AddEffectsStack();
            }
        }

        private void RemoveEffects()
        {
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                AddEffectsStack();
            }
        }

        public EffectAdderWhileTrueEffect(
            string name,
            string description,
            string targetName,
            PropTrigger addTrigger,
            PropTrigger removeTrigger,
            string resourceConditionPath,
            List<(IEffect effect, int stackCount)> effects
        ) : this(
            name,
            description,
            targetName,
            addTrigger,
            removeTrigger,
            resourceConditionPath,
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

        public EffectAdderWhileTrueEffect(
            string name,
            string description,
            string targetName,
            PropTrigger addTrigger,
            PropTrigger removeTrigger,
            string resourceConditionPath,
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
            AddTrigger = addTrigger;
            RemoveTrigger = removeTrigger;
            ResourceConditionPath = resourceConditionPath;
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
}