using UnityEngine;

namespace EntityComponents.UnitComponents.EnemyComponents
{
    public class SpriteType : MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        public EnemyType EnemyType => _enemyType;
    }
}