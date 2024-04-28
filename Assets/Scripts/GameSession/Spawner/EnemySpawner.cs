using System;
using System.Collections;
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

        [SerializeField] private int _maxSpawnAmount;
        [SerializeField] private float _secondsBetweenSpawns;

        [SerializeField] private Vector2Reference _player;

        public List<EnemyPool> enemyPools = new();

        [Header("Enemy in spawn queue")]
        [SerializeField, Tooltip("Only for editor")] private EnemyData[] _spawnQueue;

        private readonly Grouping _grouping = new();
        private readonly Circle _circle = new();
        private Queue<EnemyData> EnemiesToSpawn { get; } = new();

        public int spawnQueue;

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
            StartCoroutine(Co_Spawn());
        }

        private void OnGUI()
        {
            _spawnQueue = EnemiesToSpawn.ToArray();
        }

        #endregion

        private IEnumerator Co_Spawn()
        {
            // todo yield return WaitUntil сложность живыых противников не превышает максимальную сложность для этого времени

            while (true)
            {
                yield return new WaitForSeconds(_secondsBetweenSpawns);
                yield return new WaitUntil(() => spawnQueue > 0);
                var min = Mathf.Min(spawnQueue, _maxSpawnAmount);
                var amount = Random.Range(1, min);
                var spawn = CreateWave(amount);
                PlaceEnemies(spawn, _grouping.GetRandomMode(), _player);
                PrepareEnemies(spawn, _player);
                spawnQueue -= amount;
            }
        }

        private void SpawnFirstWave() => SpawnWave(WaveType.First);

        private void SpawnNormalWave() => SpawnWave(WaveType.Normal);

        private void SpawnWave(WaveType waveType) // add enemies to queue
        {
            if(_player is null) return;
            if (IsSpawnBlocked()) return;
            spawnQueue += _waveDataPreset.GetSize(waveType);
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
            var weightSum = enemyPools.Sum(pool => pool.GetWeigth());

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
                if (next < limit) return pool.Get(); // GET FROM POOL HERE!!!
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
                RotateEnemyToTarget(playerPosition, enemy.rigidbody2D, enemy.transform.position);
                enemy.enabled = true;
            }
        }

        private void RotateEnemyToTarget(Vector2 targetPosition, Rigidbody2D enemyRb2D, Vector2 enemyPosition)
        {
            var direction = enemyPosition - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            enemyRb2D.rotation = angle;
            enemyRb2D.SetRotation(angle);
        }
    }
}