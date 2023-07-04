using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameSession.PlayingField
{
    public class TileGenerator : MonoBehaviour
    {
        [SerializeField] private TileData[] _tiles;

        private UnityEngine.Camera _camera;
        private float _tileSize;

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
            //_tileSize = _tiles[0].tile.bounds.size.x; // Assuming all tile sprites have the same size

            GenerateTiles();
        }

        private void GenerateTiles()
        {
            var cameraHeight = _camera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * _camera.aspect;

            var numTilesX = Mathf.CeilToInt(cameraWidth / _tileSize);
            var numTilesY = Mathf.CeilToInt(cameraHeight / _tileSize);

            var bottomLeft = (Vector2)_camera.transform.position - new Vector2(cameraWidth, cameraHeight) / 2f;

            for (int y = 0; y < numTilesY; y++)
            {
                for (int x = 0; x < numTilesX; x++)
                {
                    var tilePosition = bottomLeft + new Vector2(x, y) * _tileSize;
                    var randomTile = GetRandomTile();
                    Instantiate(randomTile.prefab, tilePosition, Quaternion.identity);
                }
            }
        }

        private TileData GetRandomTile()
        {
            var totalWeight = _tiles.Sum(tile => tile.spawnWeight);

            var randomWeight = Random.Range(0f, totalWeight);
            
            foreach (var tile in _tiles)
            {
                randomWeight -= tile.spawnWeight;
                if (randomWeight <= 0f)
                {
                    return tile;
                }
            }

            return null; // Return null if no tiles are defined
        }
    }
}