using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies; 
    public float enemySpawnPerSecond = 0.5f; 
    public float enemyDefaultPadding = 1.5f;
    public GameObject timer;


    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;

    private float CalculateSpawnPerSecond()
    {
        var seconds = timer.GetComponent<Timer>().GetTotalSeconds();

        return enemySpawnPerSecond + (seconds / 30f);
    }

    public void SpawnEnemy()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null) return;
        
        //Debug.Log(CalculateSpawnPerSecond());

        var index = UnityEngine.Random.Range(0, prefabEnemies.Length);
        var spawnedGameObject = Instantiate<GameObject>(prefabEnemies[index]);

        
        camHeight = Camera.main.orthographicSize * 2;

        
        camWidth = camHeight * Camera.main.aspect;

        //Debug.Log("Width and height of m_camera= " + camWidth + " " + camHeight);

        //Get the radius of the circle circumscribed around the m_camera rectangle
        var radius = Lib2DMethods.HypotenuseLength(camHeight, camWidth) / 2;
        //Debug.Log("radius of circle= " + radius);

        Vector2 localPos = Lib2DMethods.RandOnCircle(radius);

        var xSelfPos = Lib2DMethods.PlayerPos.x;
        var ySelfPos = Lib2DMethods.PlayerPos.y;

        var xOnCirclePos = localPos.x;
        var yOnCirclePos = localPos.y;

        var xResult = xSelfPos + xOnCirclePos;
        var yResult = ySelfPos + yOnCirclePos;

        localPos.x = xResult;
        localPos.y = yResult;

        spawnedGameObject.transform.position = localPos;

        // Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f / CalculateSpawnPerSecond());
    }

    // Start is called before the first frame update
    private void Start()
    {
        Invoke("SpawnEnemy", 1f / CalculateSpawnPerSecond());
    }
}
