using Assets.Scripts.Core;

namespace Assets.Scripts.Entity.Effects
{
    public class IncreaseResourceOn : IEffect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetName { get; set; }
        public string Identifier { get; set; }

        public PropTrigger Trigger { get; set; }
        public int TriggerCount { get; set; }
        public PropTrigger IncrementalResource { get; set; }
        public int Value { get; set; }

        public bool IsTemporal { get; set; }
        public Stat.Stat Duration { get; set; }
        public bool IsDurationStacks { get; set; }
        public bool IsDurationUpdates { get; set; }

        public bool IsStacking { get; set; }
        public bool IsStackSeparateDuration { get; set; }
        public Resource StacksCount { get; set; }
        public Stat.Stat MaxStackCount { get; set; }

        private Resource _resource;
        private int _triggerCount;

        public void Attach(Entity target)
        {
            _resource = (Resource)EventHelper.GetPropByPath(target, IncrementalResource.Name);
        }

        public void Detach()
        {
        }

        public void Subscribe(Entity target)
        {
            EventHelper.AddActionByName(EventHelper.GetPropByPath(target, Trigger.Path), Trigger.Name, AddValue);
        }

        public void Unsubscribe(Entity target)
        {
            EventHelper.RemoveActionByName(EventHelper.GetPropByPath(target, Trigger.Path), Trigger.Name, AddValue);
        }

        private void AddValue()
        {
            _triggerCount++;
            if (_triggerCount < TriggerCount) return;
            for (int i = 0; i < StacksCount.GetValue(); i++)
            {
                _resource.Increase(Value);
            }
            _triggerCount = 0;
        }
    }
}