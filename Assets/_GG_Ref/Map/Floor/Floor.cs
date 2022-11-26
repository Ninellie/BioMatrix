using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;
    public TileBase tileGrass;

    private Camera mainCamera;
    private Vector2 camCenterWorld;
    private Vector3Int cellInCenterOfCam;

    private void Start()
    {
        mainCamera = Camera.main;
        camCenterWorld = mainCamera.ScreenToWorldPoint(mainCamera.pixelRect.center);
        cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
    }
    private void Update()
    {
        camCenterWorld = mainCamera.ScreenToWorldPoint(mainCamera.pixelRect.center);
        var currentCell = tilemap.WorldToCell(camCenterWorld);
        if (cellInCenterOfCam != currentCell)
        {
            BoundsInt boundsInt = GetBoundsIntFromCamera(mainCamera, tilemap, tilemapRenderer.chunkCullingBounds);
            Fill(tilemap, boundsInt, tileGrass);
        }
        cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
    }
    private void Fill(Tilemap tilemap, BoundsInt boundsInt, TileBase tile)
    {
        TileBase[] tileArray = GetTileArray(boundsInt.size.x * boundsInt.size.y, tile);

        tilemap.SetTilesBlock(boundsInt, tileArray);
    }
    private TileBase[] GetTileArray(int length, TileBase tile)
    {
        TileBase[] tileArray = new TileBase[length];

        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = tile;
        }

        return tileArray;
    }
    private BoundsInt GetBoundsIntFromCamera(Camera camera, Tilemap tilemap, Vector3 chunkCullingBounds)
    {
        int cellBoundsPadding = 5;
        Vector3Int boundsIntSize = new Vector3Int( ( ( (int)camera.pixelRect.width + (int)chunkCullingBounds.x ) / (int)tilemap.cellSize.x ) + cellBoundsPadding,
                                                   ( ( (int)camera.pixelRect.height + (int)chunkCullingBounds.y ) / (int)tilemap.cellSize.y ) + cellBoundsPadding,
                                                     1);
        Vector3 camCenterWorld = camera.ScreenToWorldPoint(camera.pixelRect.center);
        Vector3Int cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
        Vector3Int boundsIntOrigin = new(cellInCenterOfCam.x - (boundsIntSize.x / 2), cellInCenterOfCam.y - boundsIntSize.y / 2, 1);
        BoundsInt boundsInt = new BoundsInt(boundsIntOrigin, boundsIntSize);
        return boundsInt;
    }
}
