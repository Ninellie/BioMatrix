using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Dumper : MonoBehaviour
    {
        [SerializeField] private GameObject _drop;
        [field: SerializeField] public bool CanDrop { get; set; }

        public void DropBonus()
        {
            Instantiate(_drop, transform.position, Quaternion.identity);
        }
    }
}