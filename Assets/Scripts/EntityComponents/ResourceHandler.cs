using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class ResourceHandler : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<string, Resource> _resources;

        public Resource GetResourceByName(string resourceName)
        {
            return _resources[resourceName];
        }
    }
}