using UnityEngine;

public class MovementControllerSideViewEnemy
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
    private Vector2 Direction => (TargetPosition - MyPosition).normalized;
    private Vector2 TargetPosition => _target.transform.position;
    private Vector2 MyPosition => _myUnit.transform.position;
    //ACCELERATION
    private Vector2 Acceleration => Speed * Direction * AccelerationSpeed * Time.fixedDeltaTime;
    private float AccelerationSpeed => _myUnit.AccelerationSpeed.Value;
    public MovementControllerSideViewEnemy(Enemy myUnit, GameObject target)
    {
        _myUnit = myUnit;
        _target = target;
    }
    public void FixedUpdateAccelerationStep()
    {
        Velocity += Acceleration;
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
}