using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;
    public TileBase tileGrass;
    public Camera camera;

    private BoundsInt camBoundsInt;
    private Rect camRect;
    private Rect rect;
    private Vector2 camCenter;
    private Vector2 camCenterWorld;
    private Vector3 cellSize;
    private Vector3Int vector3Int;
    private Vector3Int cellInCenterOfCam;
    private int cellBoundsPadding = 2;

    private void Start()
    {
        cellSize = tilemap.cellSize;

        vector3Int.z = 1;

        Debug.Log("tilemap size: " + tilemap.size);

        Debug.Log("tilemap local bounds: " + tilemap.localBounds);

        Debug.Log("tilemap cellBounds: " + tilemap.cellBounds);
    }

    private void Update()
    {
        camRect = camera.pixelRect;
        //Debug.Log(camRect.width);
        //Debug.Log(camRect.height);
        rect.width = tilemapRenderer.chunkCullingBounds.x;
        rect.width += camRect.width;
        rect.height = tilemapRenderer.chunkCullingBounds.y;
        rect.height += camRect.height;
        camCenter = camRect.center;
        //Переделать всё чтобы пиксельРект возвразал xMin и yMin и по нему создавать вектор3 со ScreenToWorldPoint и потом WorldToCell и потом добавлять к этому паддинг
        Debug.Log("Точка центра камеры: " + camCenter);
        camCenterWorld = camera.ScreenToWorldPoint(camCenter);
        //Debug.Log(camera.ScreenToWorldPoint(camCenter));
        vector3Int.x = ( (int)rect.width / (int)cellSize.x) + cellBoundsPadding;
        //Debug.Log(vector3Int.x);
        vector3Int.y = ( (int)rect.height/ (int)cellSize.y) + cellBoundsPadding;
        //Debug.Log(vector3Int.y);
        if (cellInCenterOfCam == tilemap.WorldToCell(camCenterWorld)) { } else { Fill(); }
    }

    void Resize()
    {
        if (cellInCenterOfCam.x > tilemap.WorldToCell(camCenterWorld).x)
        {
            int sizeX = tilemap.size.x + 1;
            int sizeY = tilemap.size.y;
            tilemap.size = new Vector3Int(sizeX, sizeY, 0);
        }
        if (cellInCenterOfCam.x < tilemap.WorldToCell(camCenterWorld).x)
        {
            int sizeX = tilemap.size.x - 1;
            int sizeY = tilemap.size.y;
            tilemap.size = new Vector3Int(sizeX, sizeY, 0);
        }
        if (cellInCenterOfCam.y > tilemap.WorldToCell(camCenterWorld).y)
        {
            int sizeX = tilemap.size.x;
            int sizeY = tilemap.size.y + 1;
            tilemap.size = new Vector3Int(sizeX, sizeY, 0);
        }
        if (cellInCenterOfCam.y < tilemap.WorldToCell(camCenterWorld).y)
        {
            int sizeX = tilemap.size.x;
            int sizeY = tilemap.size.y - 1;
            tilemap.size = new Vector3Int(sizeX, sizeY, 0);
        }
        cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
        TileBase[] tileArray = new TileBase[vector3Int.x * vector3Int.y * vector3Int.z];

        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = tileGrass;
        }
        camBoundsInt = new BoundsInt(cellInCenterOfCam, vector3Int);
        Debug.Log(camBoundsInt);
        Debug.Log("Длина массива тайлов: " + tileArray.Length);
        tilemap.SetTilesBlock(camBoundsInt, tileArray);
    }

    void Fill()
    {
        cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
        TileBase[] tileArray = new TileBase[vector3Int.x * vector3Int.y * vector3Int.z];

        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = tileGrass;
        }

        Vector3Int origin = new Vector3Int(cellInCenterOfCam.x - (vector3Int.x / 2), cellInCenterOfCam.y - vector3Int.y / 2, 1);

        camBoundsInt = new BoundsInt(origin, vector3Int);
        Debug.Log("границы камеры: " + camBoundsInt);

        Debug.Log("Длина массива тайлов: " + tileArray.Length);
        tilemap.SetTilesBlock(camBoundsInt, tileArray);
    }
}
