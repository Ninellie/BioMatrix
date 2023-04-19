using UnityEngine;

public class MovementControllerPlayer
{
    private readonly Player _myUnit;
    private readonly float _knockbackSpeed = 1500;
    private float _knockbackTime = 0;
    private Vector2 _knockbackPosition = Vector2.zero;
    public Vector2 KnockbackDirection
    {
        get => _knockbackDirection;
        set => _knockbackDirection = value.normalized;
    }
    private Vector2 _knockbackDirection;
    private Vector2 MyPosition => _myUnit.transform.position;
    private float Speed =>
        _myUnit.isFireButtonPressed && !_myUnit.CurrentFirearm.Magazine.IsEmpty
            ? _myUnit.Speed.Value * SpeedScale * ShootingSpeedDecrease
            : _myUnit.Speed.Value * SpeedScale;

    private const float ShootingSpeedDecrease = 0.3f;
    private float SpeedScale
    {
        get => _speedScale;
        set
        {
            switch (value)
            {
                case >= 1:
                    _speedScale = 1f;
                    return;
                case <= 0:
                    _speedScale = 0f;
                    return;
                default:
                    _speedScale = value;
                    break;
            }
        }
    }
    private float _speedScale = 1.0f;
    private const float SpeedScaleRestoreSpeedPerSecond = 1f;
    private float SpeedScaleStep => SpeedScaleRestoreSpeedPerSecond * Time.fixedDeltaTime;
    private Vector2 MovementDirection
    {
        get => _movementDirection;
        set => _movementDirection = value.normalized;
    }
    private Vector2 _movementDirection;

    private Vector2 TrueDirection => (MovementDirection + KnockbackDirection).normalized;
    public Vector2 Velocity => TrueDirection * Speed;
    public MovementControllerPlayer(Player myUnit)
    {
        _myUnit = myUnit;
    }
    public void FixedUpdateStep()
    {
        Vector2 nextPosition = MyPosition;
        if (_knockbackTime > 0)
        {
            Vector2 difference = (MyPosition - _knockbackPosition).normalized;
            Vector2 addedPosition = difference * _knockbackSpeed * Time.fixedDeltaTime;
            nextPosition += addedPosition;
            _knockbackTime -= Time.fixedDeltaTime;
            if (_knockbackTime <= 0)
            {
                KnockbackDirection = Vector2.zero;
            }
        }

        Vector2 movementStep = MovementDirection * Speed * Time.fixedDeltaTime;

        nextPosition += movementStep;

        if (SpeedScale < 1)
        {
            SpeedScale += SpeedScaleStep;
        }
        _myUnit.Rb2D.MovePosition(nextPosition);
    }
    public void SetDirection(Vector2 direction)
    {
        MovementDirection = direction;
    }
    public void Stag()
    {
        _speedScale = 0;
    }
    public void KnockBack(Entity collisionEntity)
    {
        float thrustPower = collisionEntity.KnockbackPower.Value;
        _knockbackPosition = collisionEntity.transform.position;
        KnockbackDirection = MyPosition - _knockbackPosition;
        _knockbackTime = thrustPower / _knockbackSpeed;
        Debug.Log("Knockback");
    }
}