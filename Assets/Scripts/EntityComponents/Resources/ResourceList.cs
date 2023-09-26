using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EntityComponents.Stats;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.Resources
{
    [AddComponentMenu("Entity/ResourceList")]
    [RequireComponent(typeof(StatList))]
    public class ResourceList : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private bool _usePreset;
        [SerializeField] private ResourcePreset _preset;
        [SerializeField] private StatList _statList;
        [SerializeField] private List<Resource> _resources;

        private void Awake()
        {
            TryGetComponent<StatList>(out var statList);
            _statList = statList;
            if (!_usePreset) return;
            FillListFromPreset();
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
                var isLimited = data.maxValueStatName != StatName.None;

                if (isLimited)
                {
                    var maxValueStat = _statList.GetStat(data.maxValueStatName);
                    var resource = new Resource(data.name, data.baseValue, data.minValue, data.edgeValue,
                        maxValueStat);
                    _resources.Add(resource);
                }
                else
                {
                    var resource = new Resource(data.name, data.baseValue, data.minValue, data.edgeValue);
                    _resources.Add(resource);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            if (_resources is null) return;
            if (_resources.Count == 0) return;

            foreach (var resource in _resources)
            {
                resource.stringName = $"{resource.Name}: {resource.GetValue()}";
            }
        }
        public void OnAfterDeserialize()
        {
        }
    }
}