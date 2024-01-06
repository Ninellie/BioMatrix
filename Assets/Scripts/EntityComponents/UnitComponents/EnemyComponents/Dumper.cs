using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class Dumper : MonoBehaviour
    {
        [SerializeField] private GameObject _drop;
        public bool DropOnDisable { get; set; }

        public void DropBonus()
        {
            _drop.SetActive(true);
            _drop.transform.SetParent(null, true);
        }
    }
}