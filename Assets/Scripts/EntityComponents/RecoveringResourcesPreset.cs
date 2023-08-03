using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    [CreateAssetMenu]
    public class RecoveringResourcesPreset : ScriptableObject
    {
        public List<RecoveringResourceData> recoveringResourceDataList;
    }
}