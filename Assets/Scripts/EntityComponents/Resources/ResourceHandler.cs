using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [RequireComponent(typeof(StatHandler))]
    public class ResourceHandler : MonoBehaviour
    {
        [SerializeField] private ResourcePreset _preset;
        [SerializeField] private List<NewResource> _resources;
        [SerializeField] private StatHandler _statHandler;


        private void Awake()
        {
            _statHandler = GetComponent<StatHandler>();
        }

        private void Start()
        {
            FillListFromPreset();
        }

        public NewResource GetResourceByName(ResourceName resourceName)
        {
            return _resources.FirstOrDefault(resource => resource.Name.Equals(resourceName));
        }

        [ContextMenu("Read Preset")]
        private void FillListFromPreset()
        {
            _resources.Clear();

            foreach (var data in _preset.resources)
            {
                var isLimited = data.maxValueStatName != StatName.None;

                if (isLimited)
                {
                    var maxValueStat = _statHandler.GetStatByName(data.maxValueStatName);
                    var resource = new NewResource(data.name, data.baseValue, data.minValue, data.edgeValue,
                        maxValueStat);
                    _resources.Add(resource);
                }
                else
                {
                    var resource = new NewResource(data.name, data.baseValue, data.minValue, data.edgeValue);
                    _resources.Add(resource);
                }
            }
        }
    }
}