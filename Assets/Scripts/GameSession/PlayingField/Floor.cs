using UnityEngine;
using UnityEngine.Tilemaps;


namespace Assets.Scripts.GameSession.PlayingField
{
    public class Floor : MonoBehaviour
    {
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
        }
        
        private void Update()
        {
            _camCenterWorld = _mainCamera.ScreenToWorldPoint(_mainCamera.pixelRect.center);
            var currentCell = _tilemap.WorldToCell(_camCenterWorld);
            if (_cellInCenterOfCam != currentCell)
            {
                var boundsInt = GetBoundsIntFromCamera(_mainCamera, _tilemap, _tilemapRenderer.chunkCullingBounds);
                Fill(_tilemap, boundsInt);
            }
            _cellInCenterOfCam = _tilemap.WorldToCell(_camCenterWorld);
        }
        
        private void Fill(Tilemap tilemap, BoundsInt boundsInt)
        {
            var tileArray = GetTileArray(boundsInt.size.x * boundsInt.size.y);

            tilemap.SetTilesBlock(boundsInt, tileArray);
        }

        private TileBase[] GetTileArray(int length)
        {
            var tileArray = new TileBase[length];

            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i] = _baseTile;
            }

            return tileArray;
        }
        
        private static BoundsInt GetBoundsIntFromCamera(UnityEngine.Camera camera, Tilemap tilemap, Vector3 chunkCullingBounds)
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
