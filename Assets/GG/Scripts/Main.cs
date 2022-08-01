using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Game launched");

        Debug.Log("Trying to create a player prefab");
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        Debug.Log("Player prefab was created");

        Debug.Log("Trying to create an ememy prefab");
        Instantiate(enemyPrefab, new Vector2(0, 0), Quaternion.identity);
        Debug.Log("Enemy prefab was created");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
