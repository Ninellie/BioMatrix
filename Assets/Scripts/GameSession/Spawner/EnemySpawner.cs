using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyWaveDataPreset _waveDataPreset;
        [SerializeField] private int _secondsBetweenWaves;
        [SerializeField] private Vector2Reference _player;

        public List<EnemyPool> enemyPools = new();

        private readonly Grouping _grouping = new();
        private readonly Circle _circle = new();

        #region UnityMessages

        private void Awake()
        {
            enemyPools.Clear();
            enemyPools = GetComponentsInChildren<EnemyPool>().ToList();
        }

        private void Start()
        {
            SpawnFirstWave();
            InvokeRepeating(nameof(SpawnNormalWave), _secondsBetweenWaves, _secondsBetweenWaves);
        }

        #endregion

        private void SpawnFirstWave() => SpawnWave(WaveType.First);

        private void SpawnNormalWave() => SpawnWave(WaveType.Normal);

        private void SpawnWave(WaveType waveType)
        {
            if(_player is null) return;
            if (IsSpawnBlocked()) return;
            var waveSize = _waveDataPreset.GetSize(waveType);
            var spawn = CreateWave(waveSize);
            if (spawn is null) return;
            var mode = _grouping.GetRandomMode();

            PlaceEnemies(spawn, mode, _player);
            PrepareEnemies(spawn, _player);
        }

        private List<EnemyData> CreateWave(int waveSize)
        {
            var spawn = GetWeighedEnemyListByTime(waveSize);
            if (spawn is null) return null;
            if (spawn.Count == 0) return null;
            return spawn;
        }

        private List<EnemyData> GetWeighedEnemyListByTime(int numberOfEnemies)
        {
            var weightSum = 0f;

            foreach(var pool in enemyPools)
            {
                weightSum += pool.GetWeigth();
            }

            var enemyList = new List<EnemyData>(numberOfEnemies);

            // Отбор множества врагов по их весу
            for (int i = 0; i < enemyList.Capacity; i++)
            {
                var enemy = GetWeighedEnemyByTime(weightSum);
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        private EnemyData GetWeighedEnemyByTime(float weightSum)
        {
            var next = Random.Range(0, weightSum);
            var limit = 0f;

            foreach (var pool in enemyPools)
            {
                limit += pool.GetWeigth();
                if (next < limit) return pool.Get();
            }

            Debug.LogWarning("No enemies available for current time");
            return null;
        }

        private bool IsSpawnBlocked()
        {
            var enemyCount = 0;

            foreach (var pool in enemyPools)
            {
                enemyCount += pool.pool.CountActive;
            }
            
            var maxEnemies = _waveDataPreset.GetMaxEnemiesInScene();
            return enemyCount >= maxEnemies;
        }

        private void PlaceEnemies(IReadOnlyList<EnemyData> enemies, GroupingMode mode, Vector2 playerPosition)
        {
            float padding = 0;
            if (mode == GroupingMode.Group)
            {
                padding = enemies.Sum(enemy => enemy.GetComponent<CircleCollider2D>().radius) / 2;
            }

            var positions = _circle.GetPositions(enemies.Count, mode, playerPosition, padding);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = positions[i];
            }
        }

        private void PrepareEnemies(IEnumerable<EnemyData> enemies, Vector2 playerPosition)
        {
            foreach (var enemy in enemies)
            {
                if (enemy.spriteType.EnemyType != EnemyType.AboveView) return;
                RotateEnemyToPlayer(playerPosition, enemy.rigidbody2D, enemy.transform.position);
            }
        }

        public void RotateEnemyToPlayer(Vector2 targetPosition, Rigidbody2D rigidbody2D, Vector2 position)
        {
            var direction = position - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            rigidbody2D.rotation = angle;
            rigidbody2D.SetRotation(angle);
        }
    }
}