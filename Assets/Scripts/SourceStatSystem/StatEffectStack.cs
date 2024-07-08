using UnityEngine;

namespace SourceStatSystem
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

        private void OnValidate()
        {
            foreach (var statSource in _statSources)
            {
                statSource.Type = StatSourceType.Effect;
                statSource.PackId = name;
                StatSourcesBuilder.SetStatSourceInspectorId(statSource);
            }
        }
    }
}