using UnityEngine;


public class MovementControllerAboveViewEnemy
{
    private readonly GameObject _target;
    private readonly Enemy _myUnit;
    private Vector2 Velocity
    {
        get => _velocity;
        set
        {
            if (value.magnitude > Speed)
            {
                _velocity = value.normalized * Speed;
                return;
            }
            _velocity = value;
        }
    }
    private Vector2 _velocity;
    private float Speed => _myUnit.Speed.Value * SpeedScale;
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
    //DIRECTION
    private Vector2 ViewDirection => _myUnit.transform.up;
    private Vector2 Direction => (TargetPosition - MyPosition).normalized;
    private Vector2 TargetPosition => _target.transform.position;
    private Vector2 MyPosition => _myUnit.transform.position;
    //ACCELERATION
    private float AccelerationSpeed => _myUnit.AccelerationSpeed.Value;
    private Vector2 AccelerationStep => ViewDirection * AccelerationSpeed * Time.fixedDeltaTime;
    //ROTATION
    private float RotationSpeed => _myUnit.RotationSpeed.Value;
    private float RotationStep => RotationSpeed * Time.fixedDeltaTime;
    public MovementControllerAboveViewEnemy(Enemy myUnit, GameObject target)
    {
        _myUnit = myUnit;
        _target = target;
    }
    public void FixedUpdateAccelerationStep()
    {
        TurnToTargetStep();
        Velocity += AccelerationStep;
        _myUnit.rb2D.velocity = Velocity;
        if (SpeedScale < 1f) _speedScale += SpeedScaleStep;
    }
    public void Stag()
    {
        _speedScale = 0;
    }
    public void KnockBackFromTarget(Entity collisionEntity)
    {
        if (SpeedScale < 1f) { return; }
        Stag();
        float thrustPower = collisionEntity.KnockbackPower.Value;
        Vector2 difference = (MyPosition - TargetPosition).normalized;
        Vector2 knockbackVelocity = difference * thrustPower * _myUnit.rb2D.mass;
        _myUnit.rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
    }
    public void TurnToTarget()
    {
        var directionAngle = GetDirectionAngle(Direction);
        _myUnit.rb2D.rotation = directionAngle;
    }
    private void TurnToTargetStep()
    {
        var directionAngle = GetDirectionAngle(Direction);
        var degreesBetweenDirectionAndMe = GetDegreesBetweenAngleAndMe(directionAngle);
        if (degreesBetweenDirectionAndMe == 0) { return; }
        var turnSide = GetRotationSide();
        if (RotationStep < degreesBetweenDirectionAndMe)
        {
            _myUnit.rb2D.rotation += turnSide * RotationStep;
            return;
        }
        _myUnit.rb2D.rotation = directionAngle;
    }
    private float GetDegreesBetweenAngleAndMe(float angle)
    {
        return Mathf.Abs(angle - _myUnit.rb2D.rotation);
    }
    private int GetRotationSide()
    {
        var rotationsDif = GetDirectionAngle(Direction) - _myUnit.rb2D.rotation;
        return rotationsDif switch
        {
            //1 for right, 2 for left
            > 0 => 1,
            < 0 => -1,
            _ => 0
        };
    }
    private float GetDirectionAngle(Vector2 direction)
    {
        var angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        return angleInDegrees;
    }
}