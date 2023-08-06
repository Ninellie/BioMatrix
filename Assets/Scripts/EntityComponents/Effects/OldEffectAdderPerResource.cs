using Assets.Scripts.Core;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class OldEffectAdderPerResource : IOldEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }

        public IOldEffect OldEffect { get; set; }
        public PropTrigger TriggerResource { get; set; }

        public string Identifier { get; set; }

        public bool IsTemporal { get; set; }
        public Stats.OldStat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public OldResource StacksCount { get; set; }
        public Stats.OldStat MaxStackCount { get; set; }

        private OldResource _triggerResource;
        private int _value;
        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            _triggerResource = (OldResource)EventHelper.GetPropByPath(target, TriggerResource.Path);
            _value = _triggerResource.GetValue();
            UpdateMods();
        }

        public void Detach()
        {
            while (_value > 0)
            {
                _target.RemoveEffectStack(OldEffect);
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
                _target.AddEffectStack(OldEffect);
            }
        }

        private void RemoveStack()
        {
            for (int i = 0; i < _value; i++)
            {
                _target.RemoveEffectStack(OldEffect);
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
                            _target.AddEffectStack(OldEffect);
                            dif--;
                            break;
                        case < 0:
                            _target.RemoveEffectStack(OldEffect);
                            dif++;
                            break;
                    }
                }
            }

            _value = _triggerResource.GetValue();
        }

        public OldEffectAdderPerResource(
            string name,
            string description,
            string targetName,
            IOldEffect oldEffect,
            PropTrigger triggerResource
        ) : this(
            name,
            description,
            targetName,
            oldEffect,
            triggerResource,
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

        public OldEffectAdderPerResource(
            string name,
            string description,
            string targetName,
            IOldEffect oldEffect,
            PropTrigger triggerResource,
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
            OldEffect = oldEffect;
            TriggerResource = triggerResource;
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