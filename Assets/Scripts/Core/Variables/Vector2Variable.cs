using UnityEngine;

namespace Core.Variables
{
    [CreateAssetMenu(fileName = "New Vector2 Variable", menuName = "Variables/Vector2", order = 51)]
    public class Vector2Variable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public Vector2 value;

        public void SetValue(Vector2 value)
        {
            this.value = value;
        }

        public void SetValue(Vector2Variable value)
        {
            this.value = value.value;
        }

        public void ApplyChange(Vector2 amount)
        {
            value += amount;
        }

        public void ApplyChange(Vector2Variable amount)
        {
            value += amount.value;
        }
    }
}