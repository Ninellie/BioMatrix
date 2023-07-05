using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Assets.Scripts.GameSession.PlayingField.TilesCameraHelper;

namespace Assets.Scripts.GameSession.PlayingField
{
    public class BordersFiller : MonoBehaviour
    {
        [SerializeField] private TileData _tile;
        [SerializeField] private bool _fillAtStart;
        [SerializeField] private int _fillingFramesCount;
        [SerializeField] private float _delayBetweenFillings;

        private Tilemap _tilemap;
        private TilemapRenderer _tilemapRenderer;
        private UnityEngine.Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            _tilemap = GetComponentInChildren<Tilemap>();
            _tilemapRenderer = GetComponentInChildren<TilemapRenderer>();
        }

        private void Start()
        {
            if (!_fillAtStart) return;
            if (_tile is null) return;
            FillBorderTilesInCameraBounds();
        }

        private void FillBorderTilesInCameraBounds()
        {
            var boundsInt = GetBoundsIntFromCamera(_mainCamera, _tilemap, _tilemapRenderer.chunkCullingBounds);
            StartCoroutine(FillBounds(boundsInt));
        }

        private IEnumerator FillBounds(BoundsInt bounds)
        {
            for (int i = 0; i < _fillingFramesCount; i++)
            {

                StartCoroutine(FillFrame(_tilemap, _tile.tile, bounds, _delayBetweenFillings));

                var perimeterTilesCount = bounds.size.x * 2 + bounds.size.y * 2 - 2;
                var frameFillingDelay = perimeterTilesCount * _delayBetweenFillings;

                yield return new WaitForSeconds(frameFillingDelay * 1.1f);

                var min = bounds.min;
                min = new Vector3Int(min.x + 1, min.y + 1);
                var max = bounds.max;
                max = new Vector3Int(max.x - 1, max.y - 1);

                bounds.SetMinMax(min, max);
            }
        }

        private IEnumerator FillFrame(Tilemap tilemap, TileBase tileToFill, BoundsInt bounds, float delay)
        {
            var left = bounds.xMin;
            var right = bounds.xMax - 1;
            var bottom = bounds.yMin;
            var top = bounds.yMax - 1;

            var x = left;
            var y = bottom;

            Vector3Int tilePos;

            // Fill bottom row
            for (int i = x; i <= right; i++)
            {
                tilePos = new Vector3Int(i, bottom, 0);
                tilemap.SetTile(tilePos, tileToFill);
                yield return new WaitForSeconds(delay);
            }

            // Fill right column
            for (int i = bottom + 1; i <= top; i++)
            {
                tilePos = new Vector3Int(right, i, 0);
                tilemap.SetTile(tilePos, tileToFill);
                yield return new WaitForSeconds(delay);
            }

            // Fill top row
            if (bottom < top)
            {
                for (int i = right - 1; i >= left; i--)
                {
                    tilePos = new Vector3Int(i, top, 0);
                    tilemap.SetTile(tilePos, tileToFill);
                    yield return new WaitForSeconds(delay);
                }
            }

            // Fill left column
            if (left < right)
            {
                for (int i = top - 1; i > bottom; i--)
                {
                    tilePos = new Vector3Int(left, i, 0);
                    tilemap.SetTile(tilePos, tileToFill);
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}