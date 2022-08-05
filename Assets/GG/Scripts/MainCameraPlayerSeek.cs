using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraPlayerSeek : MonoBehaviour
{
    private Vector3 newPosition
    {
        get
        {
            var horizontal = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x;
            var vertical = GameObject.FindGameObjectsWithTag("Player")[0].transform.position.y;

            //The z-axis does not change
            return new Vector3(horizontal, vertical, -100.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = newPosition;
    }
}
