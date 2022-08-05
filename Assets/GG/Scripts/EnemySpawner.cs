using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies; // Массив шаблонов Enemy
    public float enemySpawnPerSecond = 0.5f; // Вражеских кораблей в секунду
    public float enemyDefaultPadding = 1.5f; // Отступ для позиционирования

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;
    private Vector2 newPosition
    {
        get
        {
            var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
            var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;

            return new Vector2(horizontal, vertical);
        }
    }

    private Vector2 RandOnCircle(float radius)
    {
        float randAng = UnityEngine.Random.Range(0, Mathf.PI * 2);
        return new Vector2(Mathf.Cos(randAng) * radius, Mathf.Sin(randAng) * radius);
        
    }


    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    public void SpawnEnemy()
    {
        // Выбрать случайный шаблон Enemy для создания
        int ndx = UnityEngine.Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //Get camera height
        camHeight = Camera.main.orthographicSize * 2;

        //Get camera width
        camWidth = camHeight * Camera.main.aspect;

        Debug.Log("Width and height of camera= " + camWidth + " " + camHeight);

        //Get the radius of the circle circumscribed around the camera rectangle
        float radius = HypotenuseLength(camHeight, camWidth) / 2;
        Debug.Log("radius of circle= " + radius);

        Vector2 localPos = RandOnCircle(radius);

        float xSelfPos = newPosition.x;
        float ySelfPos = newPosition.y;

        //float xOnCirclePos = RandOnCircle(radius).x;
        //float yOnCirclePos = RandOnCircle(radius).y;
        float xOnCirclePos = localPos.x;
        float yOnCirclePos = localPos.y;

        float xResult = xSelfPos + xOnCirclePos;
        float yResult = ySelfPos + yOnCirclePos;

        localPos.x = xResult;
        localPos.y = yResult;

        go.transform.position = localPos;


        //Assign a random point on the circle to a variable go
        //go.transform.position = RandOnCircle(radius);

        // Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
