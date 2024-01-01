using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.GameSession.PlayingField
{
    public class TilesFiller : MonoBehaviour
    {
        [SerializeField] private TileBase _tile;
        [SerializeField] [Range(0, 100)] private int _fillingPercent;
        [SerializeField] private bool _alwaysFill;
        
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private int _tilesPerStep;

        [SerializeField] private Vector3Int[] _positionsToFill;
        public Queue<Vector3Int> PositionsToFill { get; set; }

        private readonly System.Random _random = new();

        private void Awake()
        {
            PositionsToFill = new Queue<Vector3Int>();
        }

        private void OnGUI()
        {
            _positionsToFill = PositionsToFill.ToArray();
        }

        private void OnEnable()
        {
            StartCoroutine(Fill());
        }

        private void OnDisable()
        {
            StopCoroutine(Fill());
        }

        private IEnumerator Fill()
        {
            while (true)
            {
                yield return new WaitWhile(() => PositionsToFill.Count == 0);
                yield return new WaitForFixedUpdate();
                for (int i = 0; i < Mathf.Min(_tilesPerStep, PositionsToFill.Count); i++)
                {
                    var nextPosition = PositionsToFill.Dequeue();
                    if (_alwaysFill)
                    {
                        _tilemap.SetTile(nextPosition, _tile);
                        continue;
                    }
                    if (_random.Next(0, 100) > _fillingPercent) continue;
                    _tilemap.SetTile(nextPosition, _tile);
                }
            }
        }
    }
}