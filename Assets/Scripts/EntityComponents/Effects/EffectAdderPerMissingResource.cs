using Assets.Scripts.Core;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class EffectAdderPerMissingResource : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }

        public IEffect Effect { get; set; }
        public PropTrigger TriggerResource { get; set; }
        public PropTrigger TriggerStat { get; set; }

        public string Identifier { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.OldStat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stats.OldStat MaxStackCount { get; set; }

        private Resource _triggerResource;
        private int _lackValue;
        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            _triggerResource = (Resource)EventHelper.GetPropByPath(target, TriggerResource.Path);
            _lackValue = _triggerResource.GetLackValue();
            UpdateMods();
        }

        public void Detach()
        {
            while (_lackValue > 0)
            {
                _target.RemoveEffectStack(Effect);
                _lackValue--;
            }
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, TriggerStat.Path), 
                TriggerStat.Name, UpdateMods);
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, TriggerResource.Path),
                TriggerResource.Name, UpdateMods);
            StacksCount.IncrementEvent += AddStack;
            StacksCount.DecrementEvent += RemoveStack;
        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, TriggerResource.Path),
                TriggerResource.Name, UpdateMods);
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, TriggerStat.Path),
                TriggerStat.Name, UpdateMods);
            StacksCount.IncrementEvent -= AddStack;
            StacksCount.DecrementEvent -= RemoveStack;
        }

        private void AddStack()
        {
            for (int i = 0; i < _lackValue; i++)
            {
                _target.AddEffectStack(Effect);
            }
        }

        private void RemoveStack()
        {
            for (int i = 0; i < _lackValue; i++)
            {
                _target.RemoveEffectStack(Effect);
            }
        }

        private void UpdateMods()
        {
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                var dif = _triggerResource.GetLackValue() - _lackValue;
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

            _lackValue = _triggerResource.GetLackValue();
        }


        public EffectAdderPerMissingResource(
            string name,
            string description,
            string targetName,
            IEffect effect,
            PropTrigger triggerResource,
            PropTrigger triggerStat
        ) : this(
            name,
            description,
            targetName,
            effect,
            triggerResource,
            triggerStat,
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

        public EffectAdderPerMissingResource(
            string name,
            string description,
            string targetName,
            IEffect effect,
            PropTrigger triggerResource,
            PropTrigger triggerStat,
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
            Effect = effect;
            TriggerResource = triggerResource;
            TriggerStat = triggerStat;
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