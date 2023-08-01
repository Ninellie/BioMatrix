using Assets.Scripts.Core;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class ResourceIncreaserOnEventEffect : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }
        public string Identifier { get; set; }

        public PropTrigger Trigger { get; set; }

        public List<(int value, string resourcePath)> ResourceValues { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.OldStat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stats.OldStat MaxStackCount { get; set; }

        public Resource TriggerCountForIncreasing { get; set; }
        public Stats.OldStat MaxTriggerCountForIncreasing { get; set; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
        }

        public void Detach()
        {
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, Trigger.Path), Trigger.Name, AddTriggerCount);

            TriggerCountForIncreasing.FillEvent += IncreaseResource;
        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, Trigger.Path), Trigger.Name, AddTriggerCount);
            TriggerCountForIncreasing.FillEvent -= IncreaseResource;
        }

        private void IncreaseResource()
        {
            TriggerCountForIncreasing.Empty();
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                foreach (var tuple in ResourceValues)
                {
                    _target.IncreaseResourceValue(tuple.value, tuple.resourcePath);
                }
            }
        }

        private void AddTriggerCount()
        {
            TriggerCountForIncreasing.Increase();
        }

        public ResourceIncreaserOnEventEffect
        (
            string name,
            string description,
            string targetName,
            PropTrigger trigger,
            List<(int value, string resourcePath)> resourceValues,
            int triggerCountForIncreasing,
            bool isStacking
        ) : this
        (
            name,
            description,
            targetName,
            trigger,
            resourceValues,
            false,
            new Stats.OldStat(0, false),
            false,
            false,
            isStacking,
            false,
            new Stats.OldStat(Single.PositiveInfinity, false),
            new Stats.OldStat(triggerCountForIncreasing)
        )
        {
        }

        public ResourceIncreaserOnEventEffect
        (
            string name,
            string description,
            string targetName,
            PropTrigger trigger,
            List<(int value, string resourcePath)> resourceValues,
            bool isTemporal,
            Stats.OldStat duration,
            bool isDurationStacks,
            bool isDurationUpdates,
            bool isStacking,
            bool isStackSeparateDuration,
            Stats.OldStat maxStackCount,
            Stats.OldStat maxTriggerCountForIncreasing

        )
        {
            Name = name;
            Description = description;
            TargetName = targetName;
            Trigger = trigger;
            ResourceValues = resourceValues;
            IsTemporal = isTemporal;
            Duration = duration;
            IsDurationStacks = isDurationStacks;
            IsDurationUpdates = isDurationUpdates;
            IsStacking = isStacking;
            IsStackSeparateDuration = isStackSeparateDuration;
            MaxStackCount = maxStackCount;
            StacksCount = new Resource(MaxStackCount);
            MaxTriggerCountForIncreasing = maxTriggerCountForIncreasing;
            TriggerCountForIncreasing = new Resource(MaxTriggerCountForIncreasing);
        }
    }
}