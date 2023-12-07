using Assets.Scripts.SourceStatSystem;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class StaticStatEffect : MonoBehaviour
    {
        [SerializeField] private StatSources _list;
        [SerializeField] private StatSourceData[] _statSources;
        [SerializeField] private int _stacks;
        [SerializeField] private int _maximumStacks;
        [SerializeField] private int _appliedStacks;

        private const int MinimumStacks = 0;

        private void OnEnable()
        {
            AddAllStatSources();
        }

        private void OnDisable()
        {
            RemoveAllStatSources();
        }

        private void OnValidate()
        {
            if (!this.isActiveAndEnabled) return;

            _stacks = Mathf.Clamp(_stacks, MinimumStacks, _maximumStacks);

            if (_stacks == _appliedStacks) return;
            UpdateAppliedStacks();
        }

        private void UpdateAppliedStacks()
        {
            if (_appliedStacks > _stacks)
            {
                while (_appliedStacks != _stacks)
                {
                    RemoveStatSources();
                }
            }
            if (_appliedStacks >= _stacks) return;
            while (_appliedStacks != _stacks)
            {
                AddStatSources();
            }
        }

        public void AddStack()
        {
            if (_stacks == _maximumStacks) return;
            _stacks++;
            AddStatSources();
        }

        public void RemoveStack()
        {
            if (_stacks == MinimumStacks) return;
            RemoveStatSources();
            _stacks--;
        }

        private void AddAllStatSources()
        {
            for (int i = 0; i < _stacks; i++)
            {
                AddStatSources();
            }
        }

        private void AddStatSources()
        {
            foreach (var statSourceData in _statSources)
            {
                _list.AddStatSource(statSourceData);
            }

            _appliedStacks++;
        }

        private void RemoveAllStatSources()
        {
            for (int i = 0; i < _stacks; i++)
            {
                RemoveStatSources();
            }
        }

        private void RemoveStatSources()
        {
            foreach (var statSourceData in _statSources)
            {
                _list.RemoveStatSource(statSourceData);
            }

            _appliedStacks--;
        }
    }
}