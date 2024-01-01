using System.Collections.Generic;
using Assets.Scripts.Core.Variables.References;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GameSession.PlayingField
{
    public class EmptyFiller : MonoBehaviour
    {
        [SerializeField] private Vector2Reference _cameraCenter;
        [SerializeField] private Vector2Int _screenPixelSize = new(960, 540);
        [SerializeField] private int _margins = 6;

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TilesFiller _filler;

        [SerializeField] private List<Vector3Int> _passedPositions;
        [SerializeField] private Vector3Int _cellInCenterOfCam;

        private List<Vector3Int> _filledPositions;
        private Vector3Int _boundsSize;

        private void Awake()
        {
            _filledPositions = new List<Vector3Int>();
            _passedPositions = new List<Vector3Int>();
        }

        private void Start()
        {
            _cellInCenterOfCam = _tilemap.WorldToCell(_cameraCenter.Value);
            SendEmptyTilesInCameraBoundsToFillingQueue();
        }

        private void FixedUpdate()
        {
            FindEmptyTiles();
        }

        private void OnValidate()
        {
            DefineBounds();
        }

        private void DefineBounds()
        {
            var width = _screenPixelSize.x /= (int)_tilemap.cellSize.x;
            var height = _screenPixelSize.y /= (int)_tilemap.cellSize.y;
            width += _margins;
            height += _margins;
            _boundsSize = new Vector3Int(width, height, 1);
        }

        private void FindEmptyTiles()
        {
            var currentCell = _tilemap.WorldToCell(_cameraCenter.Value);
            if (_cellInCenterOfCam == currentCell) return;
            _cellInCenterOfCam = currentCell;
            if (_passedPositions.Contains(_cellInCenterOfCam)) return;
            _passedPositions.Add(_cellInCenterOfCam);
            SendEmptyTilesInFrameCameraToFillingQueue();
        }

        private BoundsInt GetBounds()
        {
            var width = _cellInCenterOfCam.x - _boundsSize.x / 2;
            var height = _cellInCenterOfCam.y - _boundsSize.y / 2;
            Vector3Int boundsIntOrigin = new(width, height, 0);
            return new BoundsInt(boundsIntOrigin, _boundsSize);
        }

        private void SendEmptyTilesInFrameCameraToFillingQueue()
        {
            var expandedBounds = GetBounds();

            for (int i = 0; i < expandedBounds.size.x; i++)
            {
                for (int j = 0; j < expandedBounds.size.y; j++)
                {
                    if (i < expandedBounds.size.x - 1
                     && i > 1
                     && j < expandedBounds.size.y - 1
                     && j > 1) continue;
                    var position = new Vector3Int(i + (int)expandedBounds.center.x, j + (int)expandedBounds.center.y);
                    if (_filledPositions.Contains(position)) continue;
                    _filledPositions.Add(position);
                    if (_tilemap.GetTile(position) != null) continue;
                    _filler.PositionsToFill.Enqueue(position);
                }
            }
        }

        private void SendEmptyTilesInCameraBoundsToFillingQueue()
        {
            var bounds = GetBounds();

            foreach (var position in bounds.allPositionsWithin)
            {
                if (_filledPositions.Contains(position)) continue;
                _filledPositions.Add(position);
                if (_tilemap.GetTile(position) != null) continue;
                _filler.PositionsToFill.Enqueue(position);
            }
        }
    }
}
