using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class ResourceHandler : MonoBehaviour
    {
        [SerializeField] private bool _usePreset;
        [SerializeField] private ResourcePreset _preset;
        [SerializeField] private List<NewResource> _resources;

        public void Awake()
        {
            if (!_usePreset) return;
            _resources.Clear();
            foreach (var statPresetData in _preset.resources)
            {
                var resource = new NewResource();
                resource.Set(statPresetData.baseValue);
                resource.SetName(statPresetData.name);
                _resources.Add(resource);
            }
        }

        public NewResource GetResourceByName(string resourceName)
        {
            return _resources.FirstOrDefault(resource => resource.Name.Equals(resourceName));
        }
    }
}