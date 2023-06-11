using Assets.Scripts.Core;
using System.Collections.Generic;
using Codice.CM.Client.Gui;
using System;

namespace Assets.Scripts.Entity.Effects
{
    public class IncreaserResourceOnEventEffect : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }
        public string Identifier { get; set; }

        public PropTrigger Trigger { get; set; }

        public List<(int value, string resourcePath)> ResourceValues { get; set; }

        public bool IsTemporal { get; set; }
        public Stat.Stat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stat.Stat MaxStackCount { get; set; }

        public Resource TriggerCountForIncreasing { get; set; }
        public Stat.Stat MaxTriggerCountForIncreasing { get; set; }

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
                    _target.AddResourceValue(tuple.value, tuple.resourcePath);
                }
            }
        }

        private void AddTriggerCount()
        {
            TriggerCountForIncreasing.Increase();
        }

        public IncreaserResourceOnEventEffect
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
            new Stat.Stat(0, false),
            false,
            false,
            isStacking,
            false,
            new Stat.Stat(Single.PositiveInfinity, false),
            new Stat.Stat(triggerCountForIncreasing)
        )
        {
        }

        public IncreaserResourceOnEventEffect
        (
            string name,
            string description,
            string targetName,
            PropTrigger trigger,
            List<(int value, string resourcePath)> resourceValues,
            bool isTemporal,
            Stat.Stat duration,
            bool isDurationStacks,
            bool isDurationUpdates,
            bool isStacking,
            bool isStackSeparateDuration,
            Stat.Stat maxStackCount,
            Stat.Stat maxTriggerCountForIncreasing

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