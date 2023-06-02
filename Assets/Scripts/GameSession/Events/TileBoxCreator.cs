using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GameSession.Events
{
    public class TileBoxCreator : MonoBehaviour
    {
        [SerializeField] private RuleTile _tilePrefab;
        [SerializeField] private int _borderThickness = 1;

        private Grid _grid;
        private Tilemap _tilemap;

        void Start()
        {
            _grid = GetComponent<Grid>();
            _tilemap = GetComponentInChildren<Tilemap>();
        }
        public void CreateBox()
        {
            UnityEngine.Camera mainCamera = UnityEngine.Camera.main;
            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 cameraTopRight = new Vector3(mainCamera.aspect * mainCamera.orthographicSize, mainCamera.orthographicSize, 0f) + cameraPos;
            Vector3 cameraBottomLeft = new Vector3(-mainCamera.aspect * mainCamera.orthographicSize, -mainCamera.orthographicSize, 0f) + cameraPos;

            float width = cameraTopRight.x - cameraBottomLeft.x;
            float height = cameraTopRight.y - cameraBottomLeft.y;
            Vector3 gridSize = _grid.cellSize;
        
            Vector2Int boxSize = new Vector2Int((int)(width / gridSize.x), (int)(height / gridSize.y));
            boxSize = new Vector2Int((boxSize.x + 1 + _borderThickness * 2), (boxSize.y + 1 + _borderThickness * 2));
            _tilemap.transform.position = new Vector3(cameraPos.x - boxSize.x * gridSize.x / 2, cameraPos.y - boxSize.y * gridSize.y / 2);

            for (int x = 0; x < boxSize.x; x++)
            {
                for (int y = 0; y < boxSize.y; y++)
                {
                    if (x >= 0 + _borderThickness && x < boxSize.x - _borderThickness &&
                        y >= 0 + _borderThickness && y < boxSize.y - _borderThickness) continue;
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    _tilemap.SetTile(tilePosition, _tilePrefab);
                }
            }
        }
    }
}