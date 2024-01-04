using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents
{
    public class SpriteType : MonoBehaviour
    {
        [SerializeField] private EnemyType _enemyType;
        public EnemyType EnemyType => _enemyType;
    }
}