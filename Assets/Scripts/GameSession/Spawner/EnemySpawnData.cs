using System;
using Assets.Scripts.Core.Events;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    [Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public AnimationCurve spawnWeightCurve;
        public float currentWeight; // Readonly, only for editor indication
        public GameObjectGameEvent backInPoolEvent;
        public EnemyPool pool;
    }
}