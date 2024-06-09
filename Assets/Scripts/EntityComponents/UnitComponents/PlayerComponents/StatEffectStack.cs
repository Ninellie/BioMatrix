using SourceStatSystem;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    [ExecuteInEditMode]
    public class StatEffectStack : MonoBehaviour
    {
        [SerializeField] private StatSources _list;
        [SerializeField] private StatSourceData[] _statSources;

        private void OnEnable()
        {
            foreach (var source in _statSources)
            {
                _list.AddStatSource(source);
            }
        }

        private void OnDisable()
        {
            foreach (var source in _statSources)
            {
                _list.RemoveStatSource(source);
            }
        }
    }
}