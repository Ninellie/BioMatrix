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
        [SerializeField] private float _secondsBetweenWaves;
        [SerializeField] private float _minSecondsBetweenSpawns;
        

        [SerializeField] private int _maxSpawnSize;
        [SerializeField] private float _secondsBetweenSpawns;

        [SerializeField] private Vector2Reference _player;

        public EnemyPool enemyPool;
        private readonly Grouping _grouping = new();
        private readonly Circle _circle = new();

        [SerializeField] private int _spawnQueueSize;

        #region UnityMessages

        private void Awake()
        {
            enemyPool = GetComponentInChildren<EnemyPool>();
        }

        private void Start()
        {
            EnqueueToSpawnInitial();
            InvokeRepeating(nameof(EnqueueToSpawnNormal), _secondsBetweenWaves, _secondsBetweenWaves);
            StartCoroutine(Co_Spawn());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion

        private bool IsSpawnBlocked()
        {
            if (enemyPool.GetWeight() > 0 && 
                enemyPool.pool.CountActive < _waveDataPreset.GetMaxEnemiesInScene())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private IEnumerator Co_Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(_secondsBetweenSpawns);
                yield return new WaitUntil(() => _spawnQueueSize > 0);
                yield return new WaitWhile(IsSpawnBlocked);
                Spawn();
            }
        }

        private void Spawn()
        {
            var min = Mathf.Min(_spawnQueueSize, _maxSpawnSize);
            var amount = Random.Range(1, min);
            var spawn = CreateWave(amount);
            if (spawn == null)
            {
                return;
            }

            if (spawn.Count == 0)
            {
                return;
            }

            PlaceEnemies(spawn, _grouping.GetRandomMode(), _player);
            PrepareEnemies(spawn, _player);
            _spawnQueueSize -= amount;
        }

        private void EnqueueToSpawnInitial() => EnqueueToSpawn(WaveType.First);

        private void EnqueueToSpawnNormal() => EnqueueToSpawn(WaveType.Normal);

        private void EnqueueToSpawn(WaveType waveType) // add enemies to queue
        {
            if(_player is null) return;
            if (IsSpawnBlocked()) return;
            _spawnQueueSize += _waveDataPreset.GetSize(waveType);
        }

        private List<EnemyData> CreateWave(int waveSize)
        {
            var spawn = GetWeighedEnemyListByTime(waveSize);
            if (spawn == null) return null;
            if (spawn.Count == 0) return null;
            return spawn;
        }

        private List<EnemyData> GetWeighedEnemyListByTime(int numberOfEnemies)
        {
            var weightSum = enemyPool.GetWeight();

            var enemyList = new List<EnemyData>(numberOfEnemies);

            // Отбор множества врагов по их весу
            for (int i = 0; i < enemyList.Capacity; i++)
            {
                var enemy = GetWeighedEnemyByTime(weightSum);
                if (enemy == null) continue;
                enemyList.Add(enemy);
            }

            return enemyList;
        }

        private EnemyData GetWeighedEnemyByTime(float weightSum)
        {
            var next = Random.Range(0, weightSum);
            var limit = 0f;
            limit += enemyPool.GetWeight();
            if (next < limit) return enemyPool.Get(); // GET FROM POOL HERE!!!
            Debug.LogWarning("No enemies available for current time");
            return null;
        }

        private void PlaceEnemies(IReadOnlyList<EnemyData> enemies, GroupingMode mode, Vector2 playerPosition)
        {
            if (enemies.Count == 0)
            {
                return;
            }

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