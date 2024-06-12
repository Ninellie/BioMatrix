using EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace EntityComponents.UnitComponents.Flip
{
    public enum GazeDirection
    {
        Left, Right,
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class FlipController : MonoBehaviour
    {
        [Header("Inner components")]
        [SerializeField] private MovementController _movementController;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GazeDirection _gazeDirection = GazeDirection.Left;

        private void Awake()
        {
            if (_movementController == null) _movementController = GetComponent<MovementController>();
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
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