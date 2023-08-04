using Assets.Scripts.EntityComponents.UnitComponents.Movement;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.Flip
{
    public class FlipController : MonoBehaviour
    {
        [SerializeField] private bool _isLooksToRight;

        private IMovementController _movementController;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _movementController = GetComponent<IMovementController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _spriteRenderer.flipX = _movementController.GetRawMovementDirection().x switch
            {
                < 0 => _isLooksToRight,
                > 0 => !_isLooksToRight,
                _ => _spriteRenderer.flipX
            };
        }
    }
}