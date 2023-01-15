using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
public class Movement 
{
    public float Speed { get; private set; }
    public float TurningSpeed { get; private set; }
    private Rigidbody2D _pursuingRigidbody2D;
    private readonly Unit _drivenUnit;
    private MovementState _currentState;
    private Vector2 _movementDirection;
    private readonly Rigidbody2D _drivenRigidbody2D;
    private Vector2 Velocity => _movementDirection.normalized * Speed;
    private Vector2 LocalUp => _drivenRigidbody2D.transform.up;
    public Movement(Unit drivenUnit) : this(drivenUnit, MovementState.Idle, 0)
    {
    }
    public Movement(Unit drivenUnit, float speed) : this(drivenUnit, MovementState.Idle, speed)
    {
    }
    public Movement(Unit drivenUnit, MovementState currentState, float speed)
    {
        _drivenUnit = drivenUnit;
        _currentState = currentState;
        Speed = speed;
        TurningSpeed = 1f;
        _movementDirection = Vector2.zero.normalized;
        _pursuingRigidbody2D = null;
        _drivenRigidbody2D = drivenUnit.GetComponent<Rigidbody2D>();
    }
    public void FixedUpdateMove()
    {
        Move();
    }
    private void Move()
    {
        switch (_currentState)
        {
            case MovementState.Idle:
                //DrivenRigidbody2D.velocity = Vector2.zero;
                break;
            case MovementState.Rectilinear:
                SetVelocity();
                break;
            case MovementState.Pursue:
                Pursue();
                break;
            case MovementState.Seek:
                Seek();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void ChangeState(MovementState state)
    {
        _currentState = state;
        if (state == MovementState.Idle)
        {
            _drivenRigidbody2D.velocity = Vector2.zero;
        }
    }
    public void SetPursuingTarget(GameObject pursuingTarget)
    {
        _pursuingRigidbody2D = pursuingTarget.GetComponent<Rigidbody2D>();
    }
    public void Accelerate(float speed)
    {
        if (speed >= 0)
        {
            Speed += speed;
        }
    }
    public void SlowDown(float speed)
    {
        if (speed >= 0)
        {
            Speed -= speed;
        }
    }

    public void SetMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }
    private void SetVelocity()
    {
        _drivenRigidbody2D.velocity = Velocity;
    }
    private void Pursue()
    {
        if (_pursuingRigidbody2D == null) return;
        SetMovementDirection(_pursuingRigidbody2D.position - _drivenRigidbody2D.position);
        SetVelocity();
    }
    private void Seek()
    {
        if (_pursuingRigidbody2D == null) return;
        TurnToPursuingTarget();
        SetMovementDirection(LocalUp);
        SetVelocity();
    }
    private void TurnToPursuingTarget()
    {
        var angle = GetAngleToPursuingTarget();
        var speed = TurningSpeed * Time.fixedDeltaTime;
        var lerpAngle = Mathf.LerpAngle(_drivenRigidbody2D.rotation, angle, speed);
        _drivenRigidbody2D.rotation = lerpAngle;
    }
    private float GetAngleToPursuingTarget()
    {
        var direction = GetDirectionToPursuingTarget();
        var angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        return angle;
    }
    private Vector2 GetDirectionToPursuingTarget()
    {
        if (_pursuingRigidbody2D == null) return new Vector2(0, 0);
        var vec = (Vector2)_pursuingRigidbody2D.transform.position - _drivenRigidbody2D.position;
        return vec;
    }
}