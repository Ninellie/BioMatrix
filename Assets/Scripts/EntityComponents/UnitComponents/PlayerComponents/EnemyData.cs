using System;
using EntityComponents.UnitComponents.EnemyComponents;
using UnityEngine;

namespace EntityComponents.UnitComponents.PlayerComponents
{
    public class EnemyData : MonoBehaviour
    {
        [SerializeField] private Transform enemyTransform;
        [SerializeField] private Rigidbody2D enemyRigidbody2D;
        [SerializeField] private SpriteType enemySpriteType;
        
        public Transform EnemyTransform => enemyTransform;
        public Rigidbody2D EnemyRigidbody2D => enemyRigidbody2D;
        public SpriteType EnemySpriteType => enemySpriteType;
        
        private void Awake()
        {
            if (enemyTransform == null) enemyTransform = gameObject.transform;
            if (enemyRigidbody2D == null) enemyRigidbody2D = GetComponent<Rigidbody2D>();
            if (enemySpriteType == null) enemySpriteType = GetComponent<SpriteType>();
        }
        
        private void OnValidate()
        {
            if (enemyTransform == null) enemyTransform = gameObject.transform;
            if (enemyRigidbody2D == null) enemyRigidbody2D = GetComponent<Rigidbody2D>();
            if (enemySpriteType == null) enemySpriteType = GetComponent<SpriteType>();
        }
    }
}