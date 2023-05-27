using System.Collections;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private bool _isSideView = true;
    [SerializeField] private float _rotationSpeed = 5f;

    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        Vector2 playerDirection = (_playerTransform.position - transform.position).normalized;
        if (_isSideView)
        {
            
            Vector2 movement = playerDirection * _maxSpeed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(_rigidbody2D.position + movement);
            _spriteRenderer.flipX = transform.position.x < _playerTransform.position.x;
        }
        else
        {
            var angle = GetDirectionAngle(playerDirection);

            var rotationStep = _rotationSpeed * Time.fixedDeltaTime;

            var lerpAngle = Mathf.LerpAngle(_rigidbody2D.rotation, angle, rotationStep);
            _rigidbody2D.rotation = lerpAngle;

            Vector2 movementDirection = transform.up;
            Vector2 movement = movementDirection * _maxSpeed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(_rigidbody2D.position + movement);
        }
    }
    private float GetDirectionAngle(Vector2 direction)
    {
        var angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        return angleInDegrees;
    }
}