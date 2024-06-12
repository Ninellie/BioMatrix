using EntityComponents.UnitComponents.EnemyComponents;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyData : MonoBehaviour
    {
        public Transform transform;
        public Rigidbody2D rigidbody2D;
        public SpriteType spriteType;

        private void Awake()
        {
            if (transform == null)
            {
                transform = this.gameObject.transform;
            }

            if (rigidbody2D == null)
            {
                rigidbody2D = GetComponent<Rigidbody2D>();
            }

            if (spriteType == null)
            {
                spriteType = GetComponent<SpriteType>();
            }
        }
    }
}