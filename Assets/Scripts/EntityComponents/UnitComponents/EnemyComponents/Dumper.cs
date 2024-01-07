using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Dumper : MonoBehaviour
    {
        [SerializeField] private GameObject _drop;
        [field: SerializeField] public bool CanDrop { get; set; }

        public void DropBonus()
        {
            if (!CanDrop) return;
            Instantiate(_drop, transform.position, Quaternion.identity);
        }
    }
}