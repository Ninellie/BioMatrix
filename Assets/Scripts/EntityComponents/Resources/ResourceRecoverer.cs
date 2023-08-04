using System.Collections;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [RequireComponent(typeof(ResourceHandler))]
    [RequireComponent(typeof(StatHandler))]
    public class ResourceRecoverer : MonoBehaviour
    {
        [SerializeField] private RecoveringResourcesPreset _preset;
        private ResourceHandler _resourceHandler;
        private StatHandler _statHandler;

        private void Start()
        {
            _resourceHandler = GetComponent<ResourceHandler>();
            _statHandler = GetComponent<StatHandler>();

            foreach (var data in _preset.recoveringResourceDataList)
            {
                var resource = _resourceHandler.GetResourceByName(data.resourceName);
                var recoverLimiterStat = _statHandler.GetStatByName(data.recoverLimiterStatName);
                var recoverSpeedStat = _statHandler.GetStatByName(data.recoverSpeedStatName);
                StartCoroutine(Recover(resource, recoverLimiterStat, recoverSpeedStat));
            }
        }

        private IEnumerator Recover(NewResource resource, Stat limiter, Stat valuePerSecond)
        {
            yield return new WaitForSeconds(1 / valuePerSecond.Value);
            if (resource.GetValue() < limiter.Value)
            {
                resource.Increase();
            }
        }

    }
}