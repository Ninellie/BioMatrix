using System;
using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    [Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public AnimationCurve spawnWeightCurve;
        //public AnimationCurve healthIncreaseCurve;
        public float currentWeight;
    }
}