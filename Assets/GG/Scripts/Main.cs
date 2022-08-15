using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    private void OnEnable()
    {
        Debug.Log("Game launched");
        Debug.Log("Trying to create a player prefab");
        Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        Debug.Log("Player prefab was created");
    }
}
