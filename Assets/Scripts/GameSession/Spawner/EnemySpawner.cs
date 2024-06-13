using System;
using System.Collections;
using Core.Variables.References;
using EntityComponents.UnitComponents.EnemyComponents;
using EntityComponents.UnitComponents.PlayerComponents;
using GameSession.Spawner;
using UnityEngine;

namespace Assets.Scripts.GameSession.Spawner
{
    /// <summary>
    /// Эта система спавна управляется слабо, но креативно.
    /// Вы можете задать кривую максимума врагов на сцене а также сколько требуется секунд для заполнения от нуля до этого максимума.
    /// Система сама подстроится и будет спавнить врагов с нужной скоростью для каждой определённой секунды.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Vector2Reference _playerPosition;
        [SerializeField] private EnemyPool _enemyPool;
        [Header("Spawn pattern settings")]
        [SerializeField] private SpawnDataPreset _spawnData;
        [SerializeField] private float _spawnInterval;
        [Header("Readonly Indicators")]
        [SerializeField] private int _maxEnemies;
        [SerializeField] private float _spawnQueueSize;
        [SerializeField] private float _enemiesPerSecond;
        [SerializeField] private float _enemiesPerSpawn;

        private float EnemiesPerSecond => _spawnData.MaxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad) / _spawnData.FulfillSeconds;
        private float EnemiesPerSpawn => EnemiesPerSecond * _spawnInterval;
        private readonly Circle _circle = new();

        #region UnityMessages

        private void OnGUI()
        {
            _enemiesPerSecond = EnemiesPerSecond;
            _enemiesPerSpawn = EnemiesPerSpawn;
        }

        private void Awake()
        {
            _enemyPool = GetComponentInChildren<EnemyPool>();
        }

        private void Start()
        {
            if (IsSpawnBlocked()) return;
            if (_maxEnemies == 0) return;
            _spawnQueueSize += _maxEnemies
                               / _spawnData.FulfillSeconds
                               * _spawnData.IntervalMultiplier
                               * _spawnData.FirstWaveSizeMultiplier;
        }

        private void OnEnable()
        {
            Invoke(nameof(StartWork), _spawnInterval);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion

        private void StartWork()
        {
            StartCoroutine(EnqueueToSpawn());
            StartCoroutine(Co_Spawn());
        }

        private bool IsSpawnBlocked()
        {
            _maxEnemies = (int)_spawnData.MaxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad);
            return !(_enemyPool.pool.CountActive < _maxEnemies);
        }

        private IEnumerator EnqueueToSpawn() // add enemies to spawn queue
        {
            if (_playerPosition is null)
            {
                throw new NullReferenceException($"playerPosition is null in {gameObject}");
            }
            while (true)
            {
                yield return new WaitWhile(IsSpawnBlocked);
                yield return new WaitForSeconds(_spawnData.IntervalMultiplier);
                _spawnQueueSize += _maxEnemies / (float)_spawnData.FulfillSeconds * _spawnData.IntervalMultiplier;
            }
        }

        // Эта корутина является лишь оптимизацией спавна. Она медленно спавнит врага минимально возможными группами.
        private IEnumerator Co_Spawn()
        {
            while (true)
            {
                yield return new WaitUntil(() => _spawnQueueSize >= 1);

                var spawnAmount = Mathf.Clamp(EnemiesPerSpawn, 1, _maxEnemies);
                
                for (int i = 0; i < spawnAmount; i++)
                { 
                    Spawn();
                }

                yield return new WaitForSeconds(_spawnInterval);
            }
        }

        private void Spawn()
        {
            var enemy = _enemyPool.Get();
            var radius = _circle.GetRadiusInscribedAroundTheCamera();
            radius += enemy.GetComponent<CircleCollider2D>().radius;
            var angle = _circle.GetRandomAngle();
            var enemyPos = _circle.GetPointOn(radius, _playerPosition, angle);
            enemy.EnemyTransform.position = enemyPos;
            _spawnQueueSize--;
            if (enemy.EnemySpriteType.EnemyType != EnemyType.AboveView) return;
            RotateRigidbody2DToTarget(_playerPosition, enemy.EnemyRigidbody2D);
        }

        private void RotateRigidbody2DToTarget(Vector2 targetPosition, Rigidbody2D rb2D)
        {
            var direction = rb2D.position - targetPosition;
            var angle = (Mathf.Atan2(direction.y, direction.x) + Mathf.PI / 2) * Mathf.Rad2Deg;
            rb2D.rotation = angle;
            rb2D.SetRotation(angle);
        }
    }
}