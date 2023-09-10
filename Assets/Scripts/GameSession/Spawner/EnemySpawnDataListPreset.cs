using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    [CreateAssetMenu]
    public class EnemySpawnDataListPreset : ScriptableObject
    {
        public List<EnemySpawnData> enemiesSpawnData;
    }
}