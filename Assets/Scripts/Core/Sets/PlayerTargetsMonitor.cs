using System.Linq;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using UnityEngine;

namespace Assets.Scripts.Core.Sets
{
    public class PlayerTargetsMonitor : MonoBehaviour
    {
        public PlayerTargetRuntimeSet set;
        public PlayerTarget current;

        private int _previousCount = -1;

        private void OnEnable()
        {
            UpdateCurrent();
        }

        private void Update()
        {
            if (_previousCount == set.items.Count) return;
            UpdateCurrent();
            _previousCount = set.items.Count;
        }

        public void UpdateCurrent()
        {
            current = set.items.FirstOrDefault(s => s.IsCurrent);
        }
    }
}