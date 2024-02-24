using System;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Dumper : MonoBehaviour
    {
        [SerializeField] private GameObject _drop;
        [field: SerializeField] public bool CanDrop { get; set; }

        private void OnEnable()
        {
            CanDrop = true;
        }

        public void DropBonus()
        {
            if (_drop == null) throw new NullReferenceException();
            if (!CanDrop) return;
            Instantiate(_drop, transform.position, Quaternion.identity);
        }
    }
}