using UnityEngine;

public abstract class EnemyMoveController
{
    protected GameObject Target { get; }
    protected Enemy MyUnit { get; }

    protected Vector2 Velocity
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
    protected float Speed => MyUnit.Speed.Value * SpeedScale;
    protected float SpeedScale
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
    protected float SpeedScaleStep => SpeedScaleRestoreSpeedPerSecond * Time.fixedDeltaTime;
    //DIRECTION
    protected Vector2 Direction => (TargetPosition - MyPosition).normalized;
    protected Vector2 TargetPosition => Target.transform.position;
    protected Vector2 MyPosition => MyUnit.transform.position;
    //ACCELERATION
    protected float AccelerationSpeed => MyUnit.AccelerationSpeed.Value;
    protected EnemyMoveController(Enemy myUnit, GameObject target)
    {
        this.MyUnit = myUnit;
        this.Target = target;
    }
    public abstract void FixedUpdateAccelerationStep();
    public abstract void Stag();
    public abstract void KnockBackFromTarget(Entity collisionEntity);
    public Vector2 GetMovementDirection()
    {
        return Direction;
    }
}