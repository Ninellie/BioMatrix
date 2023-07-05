using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Assets.Scripts.GameSession.PlayingField.TilesCameraHelper;

namespace Assets.Scripts.GameSession.PlayingField
{
    public class EmptyFiller : MonoBehaviour
    {
        [SerializeField]
        private TileData[] _tiles;
        
        [SerializeField]
        [Range(0, 100)]
        private float _fillingPercent;

        private Tilemap _tilemap;
        private TilemapRenderer _tilemapRenderer;
        private UnityEngine.Camera _mainCamera;
        private Vector3Int _cellInCenterOfCam;
        private List<Vector3Int>_filledPositions;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            _tilemap = GetComponentInChildren<Tilemap>();
            _tilemapRenderer = GetComponentInChildren<TilemapRenderer>();
            _filledPositions = new List<Vector3Int>();
        }

        private void Start()
        {
            var camCenterWorld = _mainCamera.ScreenToWorldPoint(_mainCamera.pixelRect.center);
            _cellInCenterOfCam = _tilemap.WorldToCell(camCenterWorld);
            FillEmptyTilesInCameraBounds();
        }
        
        private void Update()
        {
            var camCenterWorld = _mainCamera.ScreenToWorldPoint(_mainCamera.pixelRect.center);
            var currentCell = _tilemap.WorldToCell(camCenterWorld);
            if (_cellInCenterOfCam != currentCell)
            {
                FillEmptyTilesInCameraBounds();
            }
            _cellInCenterOfCam = _tilemap.WorldToCell(camCenterWorld);
        }

        private void FillEmptyTilesInCameraBounds()
        {
            var boundsInt = GetBoundsIntFromCamera(_mainCamera, _tilemap, _tilemapRenderer.chunkCullingBounds);
            FillEmptyTiles(_tilemap, boundsInt);
        }

        private void FillEmptyTiles(Tilemap tilemap, BoundsInt bounds)
        {
            var emptyTilePositions = new List<Vector3Int>();

            foreach (var position in bounds.allPositionsWithin)
            {
                if (tilemap.GetTile(position) == null && !_filledPositions.Contains(position))
                {
                    emptyTilePositions.Add(position);
                }
            }

            _filledPositions.AddRange(emptyTilePositions);

            foreach (var position in emptyTilePositions)
            {
                var randomTile = GetRandomTile(_tiles);

                var r = Random.Range(0f, 100f);
                if (r <= _fillingPercent)
                {
                    tilemap.SetTile(position, randomTile);
                }
            }
        }
    }
}
