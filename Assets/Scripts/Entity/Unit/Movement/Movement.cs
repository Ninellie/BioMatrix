using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Movement 
{
    public float Speed { get; private set; }
    private const float RotationSpeed = 1f;
    private GameObject _pursuingTarget;
    private MovementMode _mode;
    private Vector2 _movementDirection;
    private readonly Rigidbody2D _drivenRigidbody2D;
    private readonly Transform _drivenTransform;
    private Vector2 Velocity => _movementDirection.normalized * Speed;
    private Vector2 LocalUp => _drivenRigidbody2D.transform.up;
    public Movement(GameObject drivenGameObject) : this(drivenGameObject, MovementMode.Idle, 0)
    {
    }
    public Movement(GameObject drivenGameObject, float speed) : this(drivenGameObject, MovementMode.Idle, speed)
    {
    }
    public Movement(GameObject drivenGameObject, MovementMode mode, float speed)
    {
        _mode = mode;
        Speed = speed;
        _movementDirection = Vector2.zero.normalized;
        _pursuingTarget = null;
        _drivenRigidbody2D = drivenGameObject.GetComponent<Rigidbody2D>();
        _drivenTransform = drivenGameObject.transform;
    }
    public void FixedUpdateMove()
    {
        switch (_mode)
        {
            case MovementMode.Idle:
                //DrivenRigidbody2D.velocity = Vector2.zero;
                break;
            case MovementMode.Rectilinear:
                ApplyVelocity();
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
        if (mode == MovementMode.Idle)
        {
            _drivenRigidbody2D.velocity = Vector2.zero;
        }
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
    public void ChangeMovementDirection(Vector2 direction)
    {
        _movementDirection += direction;
    }
    public void SetMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }
    private void ApplyVelocity()
    {
        _drivenRigidbody2D.velocity = Velocity;
        //DrivenRigidbody2D.MovePosition(DrivenRigidbody2D.position + Velocity * Time.fixedDeltaTime);
    }
    private void Pursue()
    {
        if (_pursuingTarget == null) return;
        SetMovementDirection(_pursuingTarget.GetComponent<Rigidbody2D>().position - _drivenRigidbody2D.position);
        ApplyVelocity();
    }
    private void Seek()
    {
        if (_pursuingTarget == null) return;
        TurnToPursuingTarget();
        SetMovementDirection(LocalUp);
        ApplyVelocity();
    }
    private void LookToPursuingTarget()
    {
        var direction = GetDirectionToPursuingTarget();
        var angle = (Mathf.Atan2(direction.y, direction.x)
                     - Mathf.PI / 2) * Mathf.Rad2Deg;
        _drivenRigidbody2D.rotation = angle;
    }
    private void TurnToPursuingTarget()
    {
        var angle = GetAngleToPursuingTarget();
        var speed = RotationSpeed * Time.fixedDeltaTime;
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
        if (_pursuingTarget == null) return new Vector2(0, 0);
        var vec = (Vector2)_pursuingTarget.transform.position - _drivenRigidbody2D.position;
        return vec;
    }
}
