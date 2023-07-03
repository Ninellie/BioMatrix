using UnityEngine;

namespace Assets.Scripts.GameSession.PlayingField
{
    [System.Serializable]
    public class TileData
    {
        public Sprite sprite;
        public GameObject prefab;
        public float spawnWeight;
    }
}