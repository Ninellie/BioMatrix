using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public class RecoveringInfo
    {
        [HideInInspector]
        public string stringName;
        public ResourceName resourceName;
        public float value;
    }

    [AddComponentMenu("Entity/ResourceRecoverer")]
    [RequireComponent(typeof(ResourceList))]
    [RequireComponent(typeof(StatList))]
    public class ResourceRecoverer : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private RecoveringResourcesPreset _preset;
        [SerializeField] private List<RecoveringInfo> _recoveringResources;

        [SerializeField] [Min(0)] private float _updateRateInSeconds = 1;

        private ResourceList _resourceList;
        private StatList _statList;

        private void Awake()
        {
            _resourceList = GetComponent<ResourceList>();
            _statList = GetComponent<StatList>();
        }

        private void Start()
        {
            foreach (var data in _preset.recoveringResourceDataList)
            {
                var resource = _resourceList.GetResource(data.resourceName);
                var info = new RecoveringInfo
                {
                    resourceName = resource.Name, value = 0f
                };
                _recoveringResources.Add(info);
                var recoverLimiterStat = _statList.GetStat(data.recoverLimiterStatName);
                var recoverSpeedStat = _statList.GetStat(data.recoverSpeedStatName);
                StartCoroutine(Recover(resource, recoverLimiterStat, recoverSpeedStat, info));
            }
        }

        private IEnumerator Recover(Resource resource, Stat limiter, Stat valuePerMinute, RecoveringInfo info)
        {
            while (true)
            {
                yield return new WaitWhile(() => !(resource.GetValue() < limiter.Value));
                info.value += valuePerMinute.Value / 60 * _updateRateInSeconds;
                yield return new WaitForSeconds(_updateRateInSeconds);
                if (!(info.value > 1)) continue;
                info.value = 0f;
                resource.Increase();
            }
        }

        public void OnBeforeSerialize()
        {
            if (_recoveringResources == null) return;
            
            foreach (var recoveringResource in _recoveringResources)
            {
                recoveringResource.stringName = $"{recoveringResource.resourceName}: {recoveringResource.value}";
            }
        }
        public void OnAfterDeserialize()
        {
        }
    }
}