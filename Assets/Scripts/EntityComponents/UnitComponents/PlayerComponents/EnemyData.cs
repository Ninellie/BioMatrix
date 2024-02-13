using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyData : MonoBehaviour
    {
        public Transform transform;
        public Rigidbody2D rigidbody2D;
        public SpriteType spriteType;
    }
}