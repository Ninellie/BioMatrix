using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameObject _pursuingTarget;
    [SerializeField] private MovementMode _mode = MovementMode.Idle;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _movementDirection;

    //private bool IsIdle => _movementMode == 0;
    //private bool IsRectilinear => _movementMode == 1;
    //private bool IsPursuing => _movementMode == 2;
    //private bool IsSeeking => _movementMode == 3;

    private GameObject DrivenGameObject => gameObject;
    private Rigidbody2D DrivenRigidbody2D => DrivenGameObject.GetComponent<Rigidbody2D>();
    private Vector2 Velocity => _movementDirection.normalized * _speed;
    private void Awake()
    {
        _speed = 0;
        _movementDirection = Vector2.zero.normalized;
    }

    private void FixedUpdate()
    {
        switch (_mode)
        {
            case MovementMode.Idle:
                //Do nothing
                break;
            case MovementMode.Rectilinear:
                Move();
                break;
            case MovementMode.Pursue:
                Pursue();
                break;
            case MovementMode.Seek:
                Seek();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void ChangeMode(MovementMode mode)
    {
        _mode = mode;
    }
    public void SetPursuingTarget(GameObject pursuingTarget)
    {
        _pursuingTarget = pursuingTarget;
    }
    public void AccelerateInDirection(float speed, Vector2 direction)
    {
        ChangeMovementDirection(direction);
        Accelerate(speed);
    }
    public void Accelerate(float speed)
    {
        if (speed >= 0)
        {
            _speed += speed;
        }
        else
        {
            return;
        }
    }
    //public void SlowDown(float speed)
    //{
    //    if (speed >= 0)
    //    {
    //        _speed -= speed;
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}
    public void ChangeMovementDirection(Vector2 direction)
    {
        _movementDirection += direction;
    }
    public void SetMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }
    private void Move()
    {
        DrivenRigidbody2D.MovePosition(DrivenRigidbody2D.position + Velocity * Time.fixedDeltaTime);
    }
    private void Pursue()
    {
        SetMovementDirection(_pursuingTarget.GetComponent<Rigidbody2D>().position - DrivenRigidbody2D.position);
        Move();
    }
    private void Seek()
    {
        LookToPursuingTarget();
        Pursue();
    }
    private void LookToPursuingTarget()
    {
        float angle = (Mathf.Atan2(DirectionToPursuingTarget().y, DirectionToPursuingTarget().x) - Mathf.PI / 2) * Mathf.Rad2Deg;

        DrivenRigidbody2D.rotation = angle;
    }
    private Vector2 DirectionToPursuingTarget()
    {
        var horizontal = _pursuingTarget.transform.position.x - DrivenRigidbody2D.transform.position.x;
        var vertical = _pursuingTarget.transform.position.y - DrivenRigidbody2D.transform.position.y;

        return new Vector2(horizontal, vertical);
    }
}
