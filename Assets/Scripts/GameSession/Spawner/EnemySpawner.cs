using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameSession.Spawner
{
    public class SpawnPool : MonoBehaviour
    {

    }

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpawnDataListPreset _spawnDataPreset;
        [SerializeField] private EnemyWaveDataPreset _waveDataPreset;
        [SerializeField] private List<EnemySpawnData> _enemiesSpawnData;
        [SerializeField] private int _secondsBetweenWaves;
        [SerializeField] private Vector2Reference _player;

        private readonly Grouping _grouping = new();
        private readonly Circle _circle = new();

        private void Awake()
        {
            var preset = Instantiate(_spawnDataPreset);
            _enemiesSpawnData = preset.enemiesSpawnData;
            var pools = GetComponents<EnemyPool>();
            // Кринж, но что поделать
            foreach (var pool in pools)
            {
                foreach (var enemySpawnData in _enemiesSpawnData)
                {
                    if (pool.itemPrefab == enemySpawnData.enemyPrefab)
                    {
                        enemySpawnData.pool = pool;
                    }
                }
            }
        }

        private void Start()
        {
            SpawnFirstWave();
            InvokeRepeating(nameof(SpawnNormalWave), _secondsBetweenWaves, _secondsBetweenWaves);
        }

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

        private List<GameObject> CreateWave(int waveSize)
        {
            var spawn = GetWeighedEnemyListByTime(waveSize);
            if (spawn is null) return null;
            if (spawn.Count == 0) return null;
            return spawn;

            //var spawnedEnemies = new List<GameObject>();
            //foreach (var enemy in spawn)
            //{
            //    if (enemy is null) return null;
            //    var e = Instantiate(enemy);
            //    spawnedEnemies.Add(e);
            //}
            //return spawnedEnemies;
        }

        private List<GameObject> GetWeighedEnemyListByTime(int numberOfEnemies)
        {
            var currentTime = Time.timeSinceLevelLoad;
            var weightSum = 0f;

            foreach (var enemySpawnData in _enemiesSpawnData)
            {
                var currentEnemyWeight = enemySpawnData.spawnWeightCurve.Evaluate(currentTime);
                enemySpawnData.currentWeight = currentEnemyWeight;
                if (!(currentEnemyWeight > 0)) continue;
                weightSum += currentEnemyWeight;
            }

            var enemyList = new List<GameObject>(numberOfEnemies);

            // Отбор множества врагов по их весу
            for (int i = 0; i < enemyList.Capacity; i++)
            {
                var enemy = GetWeighedEnemyByTime(weightSum, currentTime);
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        private GameObject GetWeighedEnemyByTime(float weightSum, float time)
        {
            var next = Random.Range(0, weightSum);
            var limit = 0f;
            foreach (var enemySpawnData in _enemiesSpawnData)
            {
                var currentEnemyWeight = enemySpawnData.spawnWeightCurve.Evaluate(time);
                limit += currentEnemyWeight;
                if (next < limit) return enemySpawnData.enemyPrefab;
            }
            Debug.LogWarning("No enemies available for current time");
            return null;
        }

        private bool IsSpawnBlocked()
        {
            var enemyCount = FindObjectsOfType<SpriteType>().Length;
            var maxEnemies = _waveDataPreset.GetMaxEnemiesInScene();
            return enemyCount >= maxEnemies;
        }

        private void PlaceEnemies(IReadOnlyList<GameObject> enemies, GroupingMode mode, Vector2 playerPosition)
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

        private void PrepareEnemies(IEnumerable<GameObject> enemies, Vector2 playerPosition)
        {
            foreach (var enemy in enemies)
            {
                PrepareEnemy(enemy, playerPosition);
            }
        }

        // Transform
        // Rigidbody2D
        // SpriteType

        private void PrepareEnemy(GameObject enemy, Vector2 playerPosition)
        {
            var e = enemy.GetComponent<SpriteType>();
            if (e.EnemyType != EnemyType.AboveView) return;
            var rb2D = enemy.gameObject.GetComponent<Rigidbody2D>();
            RotateEnemyToPlayer(playerPosition, rb2D);
        }

        public void RotateEnemyToPlayer(Vector2 targetPosition, Rigidbody2D rigidbody2D)
        {
            var direction = (Vector2)rigidbody2D.transform.position - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            rigidbody2D.rotation = angle;
            rigidbody2D.SetRotation(angle);
        }
    }
}