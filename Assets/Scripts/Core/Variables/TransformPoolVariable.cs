using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Variables
{
    [CreateAssetMenu(fileName = "New Transform Pool Variable", menuName = "Variables/Transform Pool", order = 51)]
    public class TransformPoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public TransformPool value;

        public void SetValue(TransformPool value)
        {
            this.value = value;
        }

        public void SetValue(TransformPoolVariable value)
        {
            this.value = value.value;
        }
    }
}