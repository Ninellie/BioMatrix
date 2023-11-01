using UnityEngine;

namespace Assets.Scripts.SourceStatSystem
{
    [CreateAssetMenu(fileName = "New Stat", menuName = "Source Stat System/Stat", order = 51)]
    public class Stat : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; } = string.Empty;
    }
}