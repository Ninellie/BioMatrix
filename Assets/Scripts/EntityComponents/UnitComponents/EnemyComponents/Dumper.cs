using Assets.Scripts.Core.Variables.References;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Dumper : MonoBehaviour
    {
        [SerializeField] private TransformPoolReference _drops;
        [field: SerializeField] public bool CanDrop { get; set; }

        private void OnEnable()
        {
            CanDrop = true;
        }

        public void DropBonus()
        {
            if (!CanDrop) return;
            var drop = _drops.Value.Get(); // TODO Поменять TransformPool на BoonDataPool или типа того
            drop.SetPositionAndRotation(transform.position, Quaternion.identity);
            drop.gameObject.GetComponent<TrailRenderer>().Clear();
        }
    }
}