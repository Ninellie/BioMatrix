using UnityEngine;

namespace SourceStatSystem
{
    [CreateAssetMenu(fileName = "New Stat Id", menuName = "Source stat system/Stat Id", order = 51)]
    public class StatId : ScriptableObject
    {
        [field: SerializeField] public string Value { get; private set; } = string.Empty;
        [field: SerializeField] public string Description { get; private set; } = string.Empty;
    }
}