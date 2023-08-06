using Assets.Scripts.Core;

namespace Assets.Scripts.EntityComponents.Effects
{
    public class ToggleOnAttach : IOldEffect
    {
        public string Name { get; set; }
        public string Description { get; }
        public string TargetName { get; set; }

        public string TogglePropPath { get; set; }
        public bool Value { get; set; }

        public string Identifier { get; set; }

        public bool IsTemporal { get; }
        public Stats.OldStat Duration { get; }
        public bool IsDurationStacks { get; }
        public bool IsDurationUpdates { get; }

        public bool IsStacking { get; }
        public bool IsStackSeparateDuration { get; }
        public OldResource StacksCount { get; }
        public Stats.OldStat MaxStackCount { get; }

        private Entity _target;

        public void Attach(Entity target)
        {
            _target = target;
            EventHelper.SetPropValueByPath(target, TogglePropPath, Value);
        }

        public void Detach()
        {
            EventHelper.SetPropValueByPath(_target, TogglePropPath, !Value);
        }

        public void Subscribe(Entity target)
        {
        }

        public void Unsubscribe(Entity target)
        {
        }


        public ToggleOnAttach(
            string name,
            string description,
            string targetName,
            string togglePropPath,
            bool value
        ) : this(
            name,
            description,
            targetName, 
            togglePropPath, 
            value, 
            false,
            new Stats.OldStat(0, false),
            false,
            false
        )
        {
        }

        public ToggleOnAttach
        (
            string name,
            string description,
            string targetName,
            string togglePropPath,
            bool value,
            bool isTemporal,
            Stats.OldStat duration,
            bool isDurationStacks,
            bool isDurationUpdates
        )
        {
            Name = name;
            Description = description;
            TargetName = targetName;
            TogglePropPath = togglePropPath;
            Value = value;
            IsTemporal = isTemporal;
            Duration = duration;
            IsDurationStacks = isDurationStacks;
            IsDurationUpdates = isDurationUpdates;
            IsStacking = false;
            IsStackSeparateDuration = false;
            MaxStackCount = new Stats.OldStat(1, false);
            StacksCount = new OldResource(MaxStackCount);
        }
    }
}