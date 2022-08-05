using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCameraPlayerSeek : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Lib2DMethods.PlayerPos);

        transform.position = new Vector3(Lib2DMethods.PlayerPos.x, Lib2DMethods.PlayerPos.y, -100.0f);
    }
}
