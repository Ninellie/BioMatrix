using System;
using System.Collections.Generic;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class AttachResourceIncreaserEffect : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }
        public string Identifier { get; set; }

        public List<(int value, string resourcePath)> ResourceValues { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.OldStat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Stats.OldStat MaxStackCount { get; set; }
        public Resource StacksCount { get; set; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            IncreaseResource();
        }

        public void Detach()
        {
            while (!StacksCount.IsEmpty)
            {
                StacksCount.Decrease();
                DecreaseResource();
            }
        }

        public void Subscribe(Entity target)
        {
            StacksCount.IncrementEvent += IncreaseResource;
            StacksCount.DecrementEvent += DecreaseResource;
        }

        public void Unsubscribe(Entity target)
        {
            StacksCount.IncrementEvent -= IncreaseResource;
            StacksCount.DecrementEvent -= DecreaseResource;
        }

        private void IncreaseResource()
        {
            foreach (var tuple in ResourceValues)
            {
                _target.IncreaseResourceValue(tuple.value, tuple.resourcePath);
            }
        }

        private void DecreaseResource()
        {
            foreach (var tuple in ResourceValues)
            {
                _target.DecreaseResourceValue(tuple.value, tuple.resourcePath);
            }
        }

        public AttachResourceIncreaserEffect(
            string name,
            string description,
            string targetName,
            List<(int value, string resourcePath)> resourceValues,
            bool isStacking
        ) : this(
            name,
            description,
            targetName,
            resourceValues,
            false,
            new Stats.OldStat(0, false),
            false,
            false,
            isStacking,
            false,
            new Stats.OldStat(Single.PositiveInfinity, false)
        )
        {
        }
        public AttachResourceIncreaserEffect(
            string name,
            string description,
            string targetName,
            List<(int value, string resourcePath)> resourceValues
        ) : this(
            name,
            description,
            targetName,
            resourceValues,
            false,
            new Stats.OldStat(0, false),
            false,
            false,
            false,
            false,
            new Stats.OldStat(1, false)
        )
        {
        }

        public AttachResourceIncreaserEffect(
            string name,
            string description,
            string targetName,
            List<(int value, string resourcePath)> resourceValues,
            bool isTemporal,
            Stats.OldStat duration,
            bool isDurationStacks,
            bool isDurationUpdates,
            bool isStacking,
            bool isStackSeparateDuration,
            Stats.OldStat maxStackCount
        )
        {
            Name = name;
            Description = description;
            TargetName = targetName;
            ResourceValues = resourceValues;
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