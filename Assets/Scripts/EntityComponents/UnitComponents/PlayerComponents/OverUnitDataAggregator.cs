using System;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Resources;
using Assets.Scripts.EntityComponents.Stats;
using Codice.Client.GameUI.Explorer;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class ListenerData
    {
        public UnityAction action;
        public TargetName target;

        public ListenerData(UnityAction action, TargetName target)
        {
            this.action = action;
            this.target = target;
        }
    }

    public class StatListenerData : ListenerData
    {
        public StatName stat;

        public StatListenerData(UnityAction action, TargetName target, StatName stat) : base(action, target)
        {
            this.stat = stat;
        }
    }

    public class ResourceListenerData : ListenerData
    {
        public ResourceName resource;
        public ResourceEventType eventType;

        public ResourceListenerData(UnityAction action, TargetName target, ResourceName resource, ResourceEventType eventType) : base(action, target)
        {
            this.resource = resource;
            this.eventType = eventType;
        }
    }

    [AddComponentMenu("Entity/OverUnitDataAggregator")]
    public class OverUnitDataAggregator : MonoBehaviour
    {
        public Dictionary<TargetName, StatList> Stats { get; } = new();
        public Dictionary<TargetName, ResourceList> Resources { get; } = new();
        public Dictionary<TargetName, EffectsList> Effects { get; } = new();

        public void ReadInfoFromTarget(GameObject target, TargetName targetName)
        {
            Stats.Add(targetName, target.GetComponent<StatList>());
            Resources.Add(targetName, target.GetComponent<ResourceList>());
            var effectsList = target.GetComponent<EffectsList>();
            effectsList.OverUnitDataAggregator = this;
            Effects.Add(targetName, effectsList);
        }

        public void AddEffect(IEffect effect)
        {
            Effects[effect.TargetName].AddEffect(effect);
        }

        public void AddListener(ListenerData listenerData)
        {
            switch (listenerData)
            {
                case ResourceListenerData resourceListenerData:
                    AddListenerToResourceEvent(resourceListenerData);
                    break;
                case StatListenerData statListenerData:
                    AddListenerToStatEvent(statListenerData);
                    break;
            }
        }

        public void RemoveListener(ListenerData listenerData)
        {
            switch (listenerData)
            {
                case ResourceListenerData resourceListenerData:
                    RemoveListenerToResourceEvent(resourceListenerData);
                    break;
                case StatListenerData statListenerData:
                    RemoveListenerToStatEvent(statListenerData);
                    break;
            }
        }

        private void AddListenerToResourceEvent(ResourceListenerData listenerData)
        {
            var action = listenerData.action;
            var target = listenerData.target;
            var resource = listenerData.resource;
            var eventType = listenerData.eventType;

            Resources[target].GetResource(resource).AddListenerToEvent(eventType).AddListener(action);
        }

        private void AddListenerToStatEvent(StatListenerData listenerData)
        {
            var action = listenerData.action;
            var target = listenerData.target;
            var stat = listenerData.stat;

            Stats[target].GetStat(stat).valueChangedEvent.AddListener(action);
        }

        private void RemoveListenerToResourceEvent(ResourceListenerData listenerData)
        {
            var action = listenerData.action;
            var target = listenerData.target;
            var resource = listenerData.resource;
            var eventType = listenerData.eventType;

            Resources[target].GetResource(resource).AddListenerToEvent(eventType).RemoveListener(action);
        }

        private void RemoveListenerToStatEvent(StatListenerData listenerData)
        {
            var action = listenerData.action;
            var target = listenerData.target;
            var stat = listenerData.stat;

            Stats[target].GetStat(stat).valueChangedEvent.RemoveListener(action);
        }
    }
}