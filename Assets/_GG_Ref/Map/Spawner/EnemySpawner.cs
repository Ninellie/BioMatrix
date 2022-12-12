using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] enemyTypes;
    public float enemySpawnPerSecond = 0.2f;
    public float enemyDefaultPadding = 1.5f;
    public GameObject timerGameObject;
    
    private const int Complicator = 30;
    private const int EnemiesInWave = 30;
    private Timer _timer;
    private readonly System.Random _random = new();
    private void Awake()
    {
        _timer = timerGameObject.GetComponent<Timer>();
    }
    private void Start()
    {
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null) return;

        Spawn();

        Invoke("SpawnEnemy", 5f / CalculateSpawnPerSecond());
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
    private float GetCircleRadiusInscribedAroundTheCamera()
    {
        var camHeight = Camera.main.orthographicSize * 2;
        var camWidth = camHeight * Camera.main.aspect;
        return GetHypotenuseLength(camHeight, camWidth) / 2;
        
    }
    public float GetHypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
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

        PlaceEnemies(spawnedEnemies, GroupingMode.Group);
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
        }
        return selectedEnemies.ToArray();
    }
    private void PlaceEnemies(GameObject[] enemies, GroupingMode mode)
    {
        var radius = GetCircleRadiusInscribedAroundTheCamera();
        var playerPoint = (Vector2)FindObjectOfType<Player>().transform.position;
        switch (mode)
        {
            case GroupingMode.Default:
                PlaceDefaultEnemies(enemies, radius, playerPoint);
                break;
            case GroupingMode.Surround:
                PlaceRingEnemies(enemies, radius, playerPoint);
                break;
            case GroupingMode.Group:
                PlaceGroupEnemies(enemies, radius, playerPoint);
                break;
        }
    }
    private void PlaceDefaultEnemies(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
    {
        foreach (var enemy in enemies)
        {
            var randomAng = GetRandomAngle();
            enemy.transform.position = GetPointOnCircle(radius, playerPoint, randomAng);
        }
    }
    private void PlaceRingEnemies(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
    {
        var enemiesArray = enemies as GameObject[] ?? enemies.ToArray();
        var angleStep = (float)Math.PI * 2f / enemiesArray.Length;
        var nextAngle = GetRandomAngle();

        foreach (var enemy in enemiesArray)
        {
            enemy.transform.position = GetPointOnCircle(radius, playerPoint, nextAngle);
            nextAngle += angleStep;
        }
    }
    private void PlaceGroupEnemies(IEnumerable<GameObject> enemies, float radius, Vector2 playerPoint)
    {
        var enemiesArray = enemies as GameObject[] ?? enemies.ToArray();
        var padding = enemiesArray.Sum(enemy => enemy.GetComponent<CircleCollider2D>().radius) / 2;
        var packCentre = GetPointOnCircle(radius + padding, playerPoint, GetRandomAngle());
        foreach (var enemy in enemiesArray)
        {
            var v2 = new Vector2(Random.value, Random.value);
            if (Random.value > 0.5f)
            {
                enemy.transform.position = packCentre - v2;
            }
            else
            {
                enemy.transform.position = packCentre + v2;
            }
        }
    }
    private float GetRandomAngle()
    {
        return Random.Range(0, Mathf.PI * 2);
    }
    private Vector2 GetPointOnCircle(float radius, Vector2 circleCenter, float fi)
    {
        var randomPointOnBaseCircle = new Vector2(Mathf.Cos(fi) * radius, Mathf.Sin(fi) * radius);
        var randomPointOnActualCircle = circleCenter + randomPointOnBaseCircle;
        return randomPointOnActualCircle;
    }
}