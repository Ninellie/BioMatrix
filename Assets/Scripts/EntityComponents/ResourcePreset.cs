using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    [CreateAssetMenu]
    public class ResourcePreset : ScriptableObject
    {
        [SerializeField]
        public List<ResourcePresetData> resources;
    }
}