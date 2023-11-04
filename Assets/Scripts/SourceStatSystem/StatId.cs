using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [CreateAssetMenu(fileName = "New Stat Id", menuName = "Source Stat System/Stat Id", order = 51)]
    public class StatId : ScriptableObject
    {
        [field: SerializeField] public string Value { get; private set; } = string.Empty;
    }
}