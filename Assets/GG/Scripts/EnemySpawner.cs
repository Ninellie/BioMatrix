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

    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;

    public void SpawnEnemy()
    {
        int ndx = UnityEngine.Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Get camera height
        camHeight = Camera.main.orthographicSize * 2;

        //Get camera width
        camWidth = camHeight * Camera.main.aspect;

        //Debug.Log("Width and height of camera= " + camWidth + " " + camHeight);

        //Get the radius of the circle circumscribed around the camera rectangle
        float radius = Lib2DMethods.HypotenuseLength(camHeight, camWidth) / 2;
        //Debug.Log("radius of circle= " + radius);

        Vector2 localPos = Lib2DMethods.RandOnCircle(radius);

        float xSelfPos = Lib2DMethods.PlayerPos.x;
        float ySelfPos = Lib2DMethods.PlayerPos.y;

        float xOnCirclePos = localPos.x;
        float yOnCirclePos = localPos.y;

        float xResult = xSelfPos + xOnCirclePos;
        float yResult = ySelfPos + yOnCirclePos;

        localPos.x = xResult;
        localPos.y = yResult;

        go.transform.position = localPos;

        // Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
}
