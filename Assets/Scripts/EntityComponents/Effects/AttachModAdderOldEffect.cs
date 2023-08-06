using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class AttachModAdderOldEffect : IOldEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }
        public string Identifier { get; set; }

        public List<(StatModifier mod, string statPath)> Modifiers { get; set; }

        public bool IsTemporal { get; set; }
        public OldStat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public OldStat MaxStackCount { get; set; }
        public OldResource StacksCount { get; set; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            AddMods();
        }

        public void Detach()
        {
            while (!StacksCount.IsEmpty)
            {
                RemoveMods();
                StacksCount.Decrease();
            }
        }

        public void Subscribe(Entity target)
        {
            StacksCount.IncrementEvent += AddMods;
            StacksCount.DecrementEvent += RemoveMods;
        }

        public void Unsubscribe(Entity target)
        {
            StacksCount.IncrementEvent -= AddMods;
            StacksCount.DecrementEvent -= RemoveMods;
        }

        private void AddMods()
        {
            foreach (var tuple in Modifiers)
            {
                _target.AddStatModifier(tuple.mod, tuple.statPath);
            }
        }

        private void RemoveMods()
        {
            foreach (var tuple in Modifiers)
            {
                _target.RemoveStatModifier(tuple.mod, tuple.statPath);
            }
        }

        public AttachModAdderOldEffect(
            string name,
            string description,
            string targetName,
            List<(StatModifier mod, string statPath)> modifiers,
            bool isStacking
        ) : this(
            name,
            description,
            targetName,
            modifiers,
            false,
            new OldStat(0, false),
            false,
            false,
            isStacking,
            false,
            new OldStat(Single.PositiveInfinity, false)
        )
        {
        }
        public AttachModAdderOldEffect(
            string name,
            string description,
            string targetName,
            List<(StatModifier mod, string statPath)> modifiers
        ) : this(
            name,
            description,
            targetName,
            modifiers,
            false,
            new OldStat(0, false),
            false,
            false,
            false,
            false,
            new OldStat(1, false)
        )
        {
        }

        public AttachModAdderOldEffect(
            string name,
            string description,
            string targetName,
            List<(StatModifier mod, string statPath)> modifiers,
            bool isTemporal,
            OldStat duration,
            bool isDurationStacks,
            bool isDurationUpdates,
            bool isStacking,
            bool isStackSeparateDuration,
            OldStat maxStackCount
        )
        {
            Name = name;
            Description = description;
            TargetName = targetName;
            Modifiers = modifiers;
            IsTemporal = isTemporal;
            Duration = duration;
            IsDurationStacks = isDurationStacks;
            IsDurationUpdates = isDurationUpdates;
            IsStacking = isStacking;
            IsStackSeparateDuration = isStackSeparateDuration;
            MaxStackCount = maxStackCount;
            StacksCount = new OldResource(MaxStackCount);
        }
    }
}