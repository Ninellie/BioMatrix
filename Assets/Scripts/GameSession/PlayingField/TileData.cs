using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GameSession.PlayingField
{
    [System.Serializable]
    public class TileData
    {
        public TileBase tile;
        public GameObject prefab;
        public float spawnWeight;
    }
}