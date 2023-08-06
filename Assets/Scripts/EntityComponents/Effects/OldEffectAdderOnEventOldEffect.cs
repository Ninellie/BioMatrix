using Assets.Scripts.Core;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class OldEffectAdderOnEventOldEffect : IOldEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }
        public PropTrigger Trigger { get; set; }
        public string Identifier { get; set; }

        public IOldEffect OldEffect { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.OldStat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public OldResource StacksCount { get; set; }
        public Stats.OldStat MaxStackCount { get; set; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
        }

        public void Detach()
        {
            while (StacksCount.IsEmpty)
            {
                RemoveEffect();
                StacksCount.Decrease();
            }
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, Trigger.Path), Trigger.Name, AddEffect);
            StacksCount.IncrementEvent += AddEffect;
            StacksCount.DecrementEvent += RemoveEffect;
        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, Trigger.Path), Trigger.Name, AddEffect);
            StacksCount.IncrementEvent -= AddEffect;
            StacksCount.DecrementEvent -= RemoveEffect;
        }

        private void AddEffect()
        {
            _target.AddEffectStack(OldEffect);
        }

        private void RemoveEffect()
        {
            _target.RemoveEffectStack(OldEffect);
        }

        public OldEffectAdderOnEventOldEffect(
            string name,
            string description,
            string targetName,
            PropTrigger trigger,
            IOldEffect oldEffect
        ) : this(
            name,
            description,
            targetName,
            trigger,
            oldEffect,
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
        public OldEffectAdderOnEventOldEffect(
            string name,
            string description,
            string targetName,
            PropTrigger trigger,
            IOldEffect oldEffect,
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
            Trigger = trigger;
            OldEffect = oldEffect;
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