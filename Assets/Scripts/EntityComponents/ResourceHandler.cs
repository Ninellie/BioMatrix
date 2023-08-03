using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    [RequireComponent(typeof(StatHandler))]
    public class ResourceHandler : MonoBehaviour
    {
        [SerializeField] private ResourcePreset _preset;
        [SerializeField] private List<NewResource> _resources;

        private StatHandler _statHandler;

        public void Start()
        {
            _statHandler = GetComponent<StatHandler>();
            _resources.Clear();

            foreach (var data in _preset.resources)
            {
                var isLimited = string.IsNullOrEmpty(data.maxValueStatName);
                if (isLimited)
                {
                    var resource = new NewResource(data.name, data.baseValue, data.minValue, data.edgeValue);
                    _resources.Add(resource);
                }
                else
                {
                    var maxValueStat = _statHandler.GetStatByName(data.maxValueStatName);
                    var resource = new NewResource(data.name, data.baseValue, data.minValue, data.edgeValue, maxValueStat);
                    _resources.Add(resource);
                }
            }
        }

        public NewResource GetResourceByName(string resourceName)
        {
            return _resources.FirstOrDefault(resource => resource.Name.Equals(resourceName));
        }
    }
}