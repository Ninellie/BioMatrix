using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Flip
{
    public enum GazeDirection
    {
        Left, Right,
    }

    public class FlipController : MonoBehaviour
    {
        [SerializeField] private GazeDirection _gazeDirection = GazeDirection.Left;

        private IMovementController _movementController;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _movementController = GetComponent<IMovementController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            switch (_movementController.GetRawMovementDirection().x)
            {
                case < 0:
                    _spriteRenderer.flipX = _gazeDirection == GazeDirection.Right;
                    break;
                case > 0:
                    _spriteRenderer.flipX = _gazeDirection == GazeDirection.Left;
                    break;
                default:
                    _spriteRenderer.flipX = _spriteRenderer.flipX;
                    break;
            }
        }
    }
}