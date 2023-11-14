using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    [CreateAssetMenu(fileName = "New spawn", menuName = "Spawner/Spawn")]
    public class EnemySpawnDataListPreset : ScriptableObject
    {
        public List<EnemySpawnData> enemiesSpawnData;
    }
}