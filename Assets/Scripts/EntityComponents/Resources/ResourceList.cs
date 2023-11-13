using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using Assets.Scripts.SourceStatSystem;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [AddComponentMenu("Entity/ResourceList")]
    [RequireComponent(typeof(StatList))]
    public class ResourceList : MonoBehaviour
    {
        [SerializeField] private bool _usePreset;
        [SerializeField] private ResourcePreset _preset;
        [SerializeField] private StatManagerComponent _statManagerComponent;
        [SerializeField] private List<Resource> _resources;

        private void Awake()
        {
            if (_statManagerComponent == null)
            {
                TryGetComponent<StatManagerComponent>(out var statManager);
                _statManagerComponent = statManager;
            }
            if (!_usePreset) return;
            FillListFromPreset();
        }

        private void OnValidate()
        {
            if (_resources is null) return;
            if (_resources.Count == 0) return;

            foreach (var resource in _resources)
            {
                resource.stringName = $"{resource.Name}: {resource.GetValue()}";
            }
        }

        public Resource GetResource(ResourceName resourceName)
        {
            return _resources.FirstOrDefault(resource => resource.Name.Equals(resourceName));
        }

        [ContextMenu("Read Preset")]
        private void FillListFromPreset()
        {
            _resources.Clear();

            foreach (var data in _preset.resources)
            {
                var isLimited = data.isLimited;

                if (isLimited)
                {
                    var resource = new Resource(data.name, data.baseValue, data.minValue, data.edgeValue, data.maxValueStatId, _statManagerComponent);
                    _resources.Add(resource);
                }
                else
                {
                    var resource = new Resource(data.name, data.baseValue, data.minValue, data.edgeValue);
                    _resources.Add(resource);
                }
            }
        }
    }
}