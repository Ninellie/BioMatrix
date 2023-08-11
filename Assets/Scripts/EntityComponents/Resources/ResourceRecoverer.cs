using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CustomAttributes;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [Serializable]
    public class RecoveringInfo
    {
        [ReadOnly]
        public ResourceName resourceName;
        //public float secondsToRecovery;
        [ReadOnly]
        public float value;
    }

    [RequireComponent(typeof(ResourceList))]
    [RequireComponent(typeof(StatList))]
    public class ResourceRecoverer : MonoBehaviour
    {
        [SerializeField]
        private RecoveringResourcesPreset _preset;

        [SerializeField]
        private List<RecoveringInfo> _recoveringResources;

        private ResourceList _resourceList;
        private StatList _statList;

        private void Start()
        {
            _resourceList = GetComponent<ResourceList>();
            _statList = GetComponent<StatList>();

            foreach (var data in _preset.recoveringResourceDataList)
            {
                var resource = _resourceList.GetResourceByName(data.resourceName);
                var info = new RecoveringInfo
                {
                    resourceName = resource.Name, value = 0f
                };
                _recoveringResources.Add(info);
                var recoverLimiterStat = _statList.GetStatByName(data.recoverLimiterStatName);
                var recoverSpeedStat = _statList.GetStatByName(data.recoverSpeedStatName);
                StartCoroutine(Recover(resource, recoverLimiterStat, recoverSpeedStat, info));
            }
        }

        private IEnumerator Recover(Resource resource, Stat limiter, Stat valuePerSecond, RecoveringInfo info)
        {
            info.value += valuePerSecond.Value;
            yield return new WaitForSeconds(1 / valuePerSecond.Value);
            if (!(resource.GetValue() < limiter.Value)) yield break;
            info.value = 0f;
            resource.Increase();
        }

    }
}