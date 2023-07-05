using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GameSession.PlayingField
{
    public static class TilesCameraHelper
    {
        private const int CellBoundsPadding = 5;

        public static TileBase GetRandomTile(TileData[] tiles)
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

        public static BoundsInt GetBoundsIntFromCamera(UnityEngine.Camera camera, GridLayout tilemap, Vector3 chunkCullingBounds)
        {
            var boundsIntSize = new Vector3Int(
                (((int)camera.pixelRect.width + (int)chunkCullingBounds.x) / (int)tilemap.cellSize.x) + CellBoundsPadding,
                (((int)camera.pixelRect.height + (int)chunkCullingBounds.y) / (int)tilemap.cellSize.y) + CellBoundsPadding,
                1);
            var camCenterWorld = camera.ScreenToWorldPoint(camera.pixelRect.center);
            var cellInCenterOfCam = tilemap.WorldToCell(camCenterWorld);
            Vector3Int boundsIntOrigin = new(cellInCenterOfCam.x - (boundsIntSize.x / 2), cellInCenterOfCam.y - boundsIntSize.y / 2, 0);
            var boundsInt = new BoundsInt(boundsIntOrigin, boundsIntSize);
            return boundsInt;
        }
    }
}