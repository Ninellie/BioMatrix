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

    private Vector2 camCenterWorld;
    private Vector3Int cellInCenterOfCam;

    private void Start()
    {
        camCenterWorld = camera.ScreenToWorldPoint(camera.pixelRect.center);
        cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
    }

    private void Update()
    {
        camCenterWorld = camera.ScreenToWorldPoint(camera.pixelRect.center);

        if (cellInCenterOfCam == tilemap.WorldToCell(camCenterWorld)) { } else { Fill(tilemap, GetBoundsIntFromCamera(camera, tilemap, tilemapRenderer), tileGrass); }

        cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
    }

    void Fill(Tilemap tilemap, BoundsInt boundsInt, TileBase tile)
    {
        TileBase[] tileArray = GetTileArray(boundsInt.size.x * boundsInt.size.y, tile);

        tilemap.SetTilesBlock(boundsInt, tileArray);
    }

    TileBase[] GetTileArray(int length, TileBase tile)
    {
        TileBase[] tileArray = new TileBase[length];

        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = tile;
        }

        return tileArray;
    }

    BoundsInt GetBoundsIntFromCamera(Camera camera, Tilemap tilemap, TilemapRenderer tilemapRenderer)
    {
        int cellBoundsPadding = 2;
        //Делит сумму высоты-ширины камеры и высоты-ширины чанк куллинга на высоту-ширину клетки в тайлмапе и прибавляет паддинг и сохраняет результат в Vector3Int
        Vector3Int boundsIntSize = new Vector3Int( ( ( (int)camera.pixelRect.width + (int)tilemapRenderer.chunkCullingBounds.x ) / (int)tilemap.cellSize.x ) + cellBoundsPadding,
                                                   ( ( (int)camera.pixelRect.height + (int)tilemapRenderer.chunkCullingBounds.y ) / (int)tilemap.cellSize.y ) + cellBoundsPadding,
                                                     1);
        Vector3 camCenterWorld = camera.ScreenToWorldPoint(camera.pixelRect.center);
        Vector3Int cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
        Vector3Int boundsIntOrigin = new Vector3Int(cellInCenterOfCam.x - (boundsIntSize.x / 2), cellInCenterOfCam.y - boundsIntSize.y / 2, 1);
        BoundsInt boundsInt = new BoundsInt(boundsIntOrigin, boundsIntSize);
        return boundsInt;
    }
}
