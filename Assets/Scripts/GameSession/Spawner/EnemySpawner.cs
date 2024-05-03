using System;
using System.Collections;
using Assets.Scripts.Core.Variables.References;
using Assets.Scripts.EntityComponents.UnitComponents.EnemyComponents;
using Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents;
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
        //private float EnemiesPerSecond => _maxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad) / _fulfillSeconds;
        //private float WaveInterval => _fulfillSeconds / _maxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad) * _intervalMultiplier;
        [SerializeField] private Vector2Reference _playerPosition;
        [SerializeField] private EnemyPool _enemyPool;
        [Header("Spawn pattern settings")]
        [SerializeField, Min(1)] private int _fulfillSeconds;
        [SerializeField, Min(1), Tooltip("Управляет величиной интервала и количеством врагов в волне")] private int _intervalMultiplier;
        [SerializeField] private int _firstWaveSizeMultiplier;
        [SerializeField] private AnimationCurve _maxEnemiesOnScreen;
        [Header("Readonly Indicators")]
        [SerializeField] private int _maxEnemies;
        [SerializeField] private float _waveIntervalPerEnemyInSeconds;
        [SerializeField] private float _spawnQueueSize;

        private readonly Circle _circle = new();

        #region UnityMessages

        private void Awake()
        {
            _enemyPool = GetComponentInChildren<EnemyPool>();
        }

        private void Start()
        {
            if (!IsSpawnBlocked())
            {
                _spawnQueueSize += _maxEnemies / _fulfillSeconds * _intervalMultiplier * _firstWaveSizeMultiplier;
            }
            StartCoroutine(EnqueueToSpawn());
            StartCoroutine(Co_Spawn());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion

        private bool IsSpawnBlocked()
        {
            _maxEnemies = (int)_maxEnemiesOnScreen.Evaluate(Time.timeSinceLevelLoad);
            return !(_enemyPool.pool.CountActive < _maxEnemies);
        }

        private IEnumerator EnqueueToSpawn() // add enemies to queue
        {
            if (_playerPosition is null)
            {
                throw new NullReferenceException($"playerPosition is null in {gameObject}");
            }
            while (true)
            {
                yield return new WaitWhile(IsSpawnBlocked);
                yield return new WaitForSeconds(_intervalMultiplier);
                _spawnQueueSize += _maxEnemies / (float)_fulfillSeconds * _intervalMultiplier;
            }
        }

        private IEnumerator Co_Spawn()
        {
            while (true)
            {
                yield return new WaitUntil(() => _spawnQueueSize >= 1);
                _waveIntervalPerEnemyInSeconds = (float)_fulfillSeconds / _maxEnemies;
                yield return new WaitForSeconds(_waveIntervalPerEnemyInSeconds);
                Spawn();
            }
        }

        private void Spawn()
        {
            var enemy = _enemyPool.Get();
            var radius = _circle.GetRadiusInscribedAroundTheCamera();
            radius += enemy.GetComponent<CircleCollider2D>().radius;
            var angle = _circle.GetRandomAngle();
            var enemyPos = _circle.GetPointOn(radius, _playerPosition, angle);
            enemy.transform.position = enemyPos;
            _spawnQueueSize--;
            if (enemy.spriteType.EnemyType != EnemyType.AboveView) return;
            RotateEnemyToTarget(_playerPosition, enemy.rigidbody2D, enemy.transform.position);
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