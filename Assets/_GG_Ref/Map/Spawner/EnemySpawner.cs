using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] enemyTypes;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public GameObject timerGameObject;

    private const int Complicator = 30;
    private const int EnemiesInWave = 1;
    private Timer _timer;
    private readonly System.Random _random = new();
    private void Awake()
    {
        _timer = timerGameObject.GetComponent<Timer>();
    }
    private void Start()
    {
        SpawnEnemy();
        Invoke("SpawnEnemy", 1f / CalculateSpawnPerSecond());
    }
    public void SpawnEnemy()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null) return;

        Spawn();

        Invoke("SpawnEnemy", 1f / CalculateSpawnPerSecond());
    }
    private void ImproveAccordingToTimer(Enemy enemy)
    {
        enemy.LevelUp(GetTimerBonus(_timer));
        enemy.RestoreLifePoints();
    }
    private int GetTimerBonus(Timer timer)
    {
        return (int)timer.GetTotalSeconds() / Complicator;
    }
    private Vector2 GetRandomPointOnCircle(float radius, Vector2 circleCenter)
    {
        var randomPointOnBaseCircle = Lib2DMethods.RandOnCircle(radius);
        var randomPointOnActualCircle = circleCenter + randomPointOnBaseCircle;
        return randomPointOnActualCircle;
    }
    private float GetCircleRadiusInscribedAroundTheCamera()
    {
        var camHeight = Camera.main.orthographicSize * 2;
        var camWidth = camHeight * Camera.main.aspect;
        return Lib2DMethods.HypotenuseLength(camHeight, camWidth) / 2;
    }
    private float CalculateSpawnPerSecond()
    {
        var seconds = _timer.GetTotalSeconds();
        return enemySpawnPerSecond + (seconds / Complicator);
    }
    private void Spawn()
    {

        var spawn = GetEnemyList(EnemiesInWave, enemyTypes);
        
        var spawnedEnemies = new GameObject[EnemiesInWave];

        for (var i = 0; i < spawn.Length; i++)
        {
            spawnedEnemies[i] = Instantiate<GameObject>(spawn[i]);
        }

        foreach (var enemy in spawnedEnemies)
        {
            ImproveAccordingToTimer(enemy.GetComponent<Enemy>());
        }

        PlaceEnemies(spawnedEnemies, GroupingMode.Default);
    }
    private GameObject GetRandomEnemyFromList(List<GameObject> enemyList)
    {
        var sum = 0;
        foreach (var enemy in enemyList)
        {
            sum += (int)enemy.GetComponent<Enemy>().SpawnWeight.Value;
        }

        var next = _random.Next(sum);

        var limit = 0;

        foreach (var enemy in enemyList)
        {
            limit += (int)enemy.GetComponent<Enemy>().SpawnWeight.Value;
            if (next < limit)
            {
                return enemy;
            }
        }
        throw new InvalidOperationException("");
    }
    private GameObject[] GetEnemyList(int numberOfEnemies, IEnumerable<GameObject> _enemyTypes)
    {
        var enemyList = _enemyTypes.ToList();

        List<GameObject> selectedEnemies = new();

        for (var i = 0; i < numberOfEnemies; i++)
        {
            var randomEnemyFromList = GetRandomEnemyFromList(enemyList);
            selectedEnemies.Add(randomEnemyFromList);
            //enemyList.Remove(randomEnemyFromList);
        }
        return selectedEnemies.ToArray();
    }
    private void PlaceEnemies(GameObject[] enemies, GroupingMode mode)
    {
        switch (mode)
        {
            case GroupingMode.Default:
                PlaceDefaultEnemies(enemies);
                break;
            case GroupingMode.Surround:
                PlaceRingEnemies(enemies);
                break;
            case GroupingMode.Group:
                PlaceGroupEnemies(enemies);
                break;
        }
    }
    private void PlaceDefaultEnemies(IEnumerable<GameObject> enemies)
    {
        var radius = GetCircleRadiusInscribedAroundTheCamera();
        var playerPoint = Lib2DMethods.PlayerPosition;

        foreach (var enemy in enemies)
        {
            enemy.transform.position = GetRandomPointOnCircle(radius, playerPoint);
        }
    }
    private void PlaceRingEnemies(GameObject[] arrayEnemies)
    {
        var radius = GetCircleRadiusInscribedAroundTheCamera();
        var playerPoint = Lib2DMethods.PlayerPosition;
    }
    private void PlaceGroupEnemies(GameObject[] arrayEnemies)
    {
        var radius = GetCircleRadiusInscribedAroundTheCamera();
        var playerPoint = Lib2DMethods.PlayerPosition;
    }
}
