using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Tilemap tilemap;

    private void Start()
    {
        Debug.Log("tilemap size: " + tilemap.size);

        Debug.Log("tilemap local bounds: " + tilemap.localBounds);

        Debug.Log("tilemap cellBounds: " + tilemap.cellBounds);
    }
}
