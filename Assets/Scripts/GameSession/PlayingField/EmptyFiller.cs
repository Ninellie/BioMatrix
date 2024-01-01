using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Variables.References;
using Unity.Collections;
using Unity.Jobs;
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
        [SerializeField] private TilesFiller[] _fillers;

        [SerializeField] private List<Vector3Int> _passedPositions;
        [SerializeField] private Vector3Int _cellInCenterOfCam;

        //private List<Vector3Int> _filledPositions;
        private Vector3Int _boundsSize;

        private void Awake()
        {
            //_filledPositions = new List<Vector3Int>();
            _passedPositions = new List<Vector3Int>();
        }

        private void Start()
        {
            _cellInCenterOfCam = _tilemap.WorldToCell(_cameraCenter.Value);
            SendBoundsToFilling();
        }

        private void FixedUpdate()
        {
            if (IsFillingRequired())
            {
                SendEmptyTilePositionsToFilling();
            }
        }

        private void OnValidate()
        {
            DefineBounds();
        }

        private void DefineBounds()
        {
            var width = _screenPixelSize.x / (int)_tilemap.cellSize.x;
            var height = _screenPixelSize.y / (int)_tilemap.cellSize.y;
            width += _margins;
            height += _margins;
            _boundsSize = new Vector3Int(width, height, 1);
        }

        private bool IsFillingRequired()
        {
            var currentCell = _tilemap.WorldToCell(_cameraCenter.Value);
            if (_cellInCenterOfCam == currentCell) return false;
            _cellInCenterOfCam = currentCell;
            if (_passedPositions.Contains(_cellInCenterOfCam)) return false;
            _passedPositions.Add(_cellInCenterOfCam);
            return true;
        }

        private BoundsInt GetBounds()
        {
            var width = _cellInCenterOfCam.x - _boundsSize.x / 2;
            var height = _cellInCenterOfCam.y - _boundsSize.y / 2;
            Vector3Int boundsIntOrigin = new(width, height, 0);
            return new BoundsInt(boundsIntOrigin, _boundsSize);
        }

        private void SendEmptyTilePositionsToFilling()
        {
            var expandedBounds = GetBounds();

            for (int i = 0; i < expandedBounds.size.x; i++)
            {
                for (int j = 0; j < expandedBounds.size.y; j++)
                {
                    TrySendPositionToFill(i, expandedBounds, j);
                }
            }
        }

        private void TrySendPositionToFill(int i, BoundsInt expandedBounds, int j)
        {
            if (i < expandedBounds.size.x - 1 && i > 1 &&
                j < expandedBounds.size.y - 1 && j > 1) return;

            var position = new Vector3Int(i + expandedBounds.x, j + expandedBounds.y);
            //if (_filledPositions.Contains(position)) return;
            //_filledPositions.Add(position);
            if (_tilemap.GetTile(position) != null) return;
            foreach (var tilesFiller in _fillers)
            {
                tilesFiller.PositionsToFill.Enqueue(position);
            }
        }

        private void SendBoundsToFilling()
        {
            var bounds = GetBounds();
            Debug.Log($"FILL BOUNDS {bounds}");
            Debug.Log($"positions {bounds.allPositionsWithin}");

            foreach (var position in bounds.allPositionsWithin)
            {
                Debug.Log($"Each");
                //if (_filledPositions.Contains(position)) continue;
                //Debug.Log($"new pos");
                //_filledPositions.Add(position);
                if (_tilemap.GetTile(position) != null) continue;
                Debug.Log($"null tile pos");
                foreach (var tilesFiller in _fillers)
                {
                    Debug.Log($"Send");
                    tilesFiller.PositionsToFill.Enqueue(position);
                }
            }
        }
    }
}