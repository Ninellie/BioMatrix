using Assets.Scripts.Core;

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

        public IEffect Effect { get; set; }

        public string Identifier { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.Stat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stats.Stat MaxStackCount { get; set; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            var isAddCondition = (bool)EventHelper.GetPropByPath(target, ResourceConditionPath);
            if (isAddCondition)
            {
                AddEffect();
            }
        }

        public void Detach()
        {
            RemoveEffect();
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, AddTrigger.Path), AddTrigger.Name, AddEffect);
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveEffect);
        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, RemoveTrigger.Path), RemoveTrigger.Name, RemoveEffect);
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, AddTrigger.Path), AddTrigger.Name, AddEffect);
        }
    
        private void AddEffect()
        {
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                _target.AddEffectStack(Effect);
            }
        }

        private void RemoveEffect()
        {
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                _target.RemoveEffectStack(Effect);
            }
        }

        public EffectAdderWhileTrueEffect(
            string name,
            string description,
            string targetName,
            PropTrigger addTrigger,
            PropTrigger removeTrigger,
            string resourceConditionPath,
            IEffect effect
        ) : this(
            name,
            description,
            targetName,
            addTrigger,
            removeTrigger,
            resourceConditionPath,
            effect,
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

        public EffectAdderWhileTrueEffect(
            string name,
            string description,
            string targetName,
            PropTrigger addTrigger,
            PropTrigger removeTrigger,
            string resourceConditionPath,
            IEffect effect,
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
            AddTrigger = addTrigger;
            RemoveTrigger = removeTrigger;
            ResourceConditionPath = resourceConditionPath;
            Effect = effect;
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