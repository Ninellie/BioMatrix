using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private SpriteRenderer _spriteRenderer;
    private Vector2 _direction;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        var movement = _direction.normalized * _moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
    public void OnMove(InputValue input)
    {
        var inputVector2 = input.Get<Vector2>();
        _direction = inputVector2.normalized;
        _spriteRenderer.flipX = inputVector2.x switch
        {
            < 0 => true,
            > 0 => false,
            _ => _spriteRenderer.flipX
        };
    }
}
