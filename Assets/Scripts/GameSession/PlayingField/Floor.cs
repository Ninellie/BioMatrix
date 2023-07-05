using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Assets.Scripts.GameSession.PlayingField
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private TileData[] _tiles;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilemapRenderer _tilemapRenderer;
        [SerializeField] private TileBase _baseTile;

        private UnityEngine.Camera _mainCamera;
        private Vector2 _camCenterWorld;
        private Vector3Int _cellInCenterOfCam;
        private const int CellBoundsPadding = 5;

        private void Start()
        {
            _mainCamera = UnityEngine.Camera.main;
            _camCenterWorld = _mainCamera.ScreenToWorldPoint(_mainCamera.pixelRect.center);
            _cellInCenterOfCam = _tilemap.WorldToCell(_camCenterWorld);
            FillEmptyTilesInCameraBounds();
        }
        
        private void Update()
        {
            _camCenterWorld = _mainCamera.ScreenToWorldPoint(_mainCamera.pixelRect.center);
            var currentCell = _tilemap.WorldToCell(_camCenterWorld);
            if (_cellInCenterOfCam != currentCell)
            {
                FillEmptyTilesInCameraBounds();
            }
            _cellInCenterOfCam = _tilemap.WorldToCell(_camCenterWorld);
        }

        private void FillEmptyTilesInCameraBounds()
        {
            var boundsInt = GetBoundsIntFromCamera(_mainCamera, _tilemap, _tilemapRenderer.chunkCullingBounds);
            FillEmptyTiles(_tilemap, boundsInt);
        }

        private void FillBounds(BoundsInt fillingBounds)
        {
            var bounds = fillingBounds;
            const int minEmptySpace = 4;
            var x = bounds.xMax - bounds.xMin;
            var y = bounds.yMax - bounds.yMin;

            var iterationCount = (x - minEmptySpace) / 2;

            if (x / minEmptySpace < 0 || y / minEmptySpace < 0)
            {
                return;
            }

            var min = bounds.min;

            var currentPos = min;

            const int fillingDirection = 4;
            for (int i = 0; i < iterationCount; i++)
            {
                for (int j = 0; j < fillingDirection; j++)
                {
                    switch (j)
                    {
                        case 0:
                            while (currentPos.y < bounds.yMax)
                            {
                                FillTile(currentPos);
                                currentPos = new Vector3Int(currentPos.x, currentPos.y + 1, currentPos.z);
                            }
                            break;
                        case 1:
                            while (currentPos.x < bounds.xMax)
                            {
                                FillTile(currentPos);
                                currentPos = new Vector3Int(currentPos.x + 1, currentPos.y, currentPos.z);
                            }
                            break;
                        case 2:
                            while (currentPos.y > bounds.yMin)
                            {
                                FillTile(currentPos);
                                currentPos = new Vector3Int(currentPos.x, currentPos.y - 1, currentPos.z);
                            }
                            break;
                        case 3:
                            while (currentPos.x > bounds.xMin)
                            {
                                FillTile(currentPos);
                                currentPos = new Vector3Int(currentPos.x, currentPos.x + 1, currentPos.z);
                            }
                            break;
                    }
                }

                bounds = new BoundsInt(currentPos, new Vector3Int(bounds.xMax - (i + 1), bounds.yMax - (i + 1)));
            }
        }

        private void FillTile(Vector3Int position)
        {
            var tile = GetRandomTile(_tiles);
            _tilemap.SetTile(position, tile);
        }

        private void FillEmptyTiles(Tilemap tilemap, BoundsInt bounds)
        {
            var emptyTilePositions = new List<Vector3Int>();

            foreach (var position in bounds.allPositionsWithin)
            {
                if (tilemap.GetTile(position) == null)
                {
                    emptyTilePositions.Add(position);
                }
            }

            foreach (var position in emptyTilePositions)
            {
                var randomTile = GetRandomTile(_tiles);
                tilemap.SetTile(position, randomTile);
            }
        }

        private static TileBase GetRandomTile(TileData[] tiles)
        {
            var totalWeight = tiles.Sum(tile => tile.spawnWeight);

            var randomWeight = Random.Range(0f, totalWeight);

            foreach (var tile in tiles)
            {
                randomWeight -= tile.spawnWeight;
                if (randomWeight <= 0f)
                {
                    return tile.tile;
                }
            }

            return null; // Return null if no tiles are defined
        }

        private static BoundsInt GetBoundsIntFromCamera(UnityEngine.Camera camera, GridLayout tilemap, Vector3 chunkCullingBounds)
        {
            var boundsIntSize = new Vector3Int(
                (((int)camera.pixelRect.width + (int)chunkCullingBounds.x) / (int)tilemap.cellSize.x) + CellBoundsPadding,
                (((int)camera.pixelRect.height + (int)chunkCullingBounds.y) / (int)tilemap.cellSize.y) + CellBoundsPadding,
                1);
            var camCenterWorld = camera.ScreenToWorldPoint(camera.pixelRect.center);
            var cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
            Vector3Int boundsIntOrigin = new(cellInCenterOfCam.x - (boundsIntSize.x / 2), cellInCenterOfCam.y - boundsIntSize.y / 2, 1);
            var boundsInt = new BoundsInt(boundsIntOrigin, boundsIntSize);
            return boundsInt;
        }
    }
}
