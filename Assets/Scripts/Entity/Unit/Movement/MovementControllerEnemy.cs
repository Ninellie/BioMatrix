using UnityEngine;

public abstract class MovementControllerEnemy
{
    protected readonly GameObject target;
    protected readonly Enemy myUnit;
    protected Vector2 Velocity
    {
        get => velocity;
        set
        {
            if (value.magnitude > Speed)
            {
                velocity = value.normalized * Speed;
                return;
            }
            velocity = value;
        }
    }
    protected Vector2 velocity;
    protected float Speed => myUnit.Speed.Value * SpeedScale;
    protected float SpeedScale
    {
        get => speedScale;
        set
        {
            switch (value)
            {
                case >= 1:
                    speedScale = 1f;
                    return;
                case <= 0:
                    speedScale = 0f;
                    return;
                default:
                    speedScale = value;
                    break;
            }
        }
    }
    protected float speedScale = 1.0f;
    protected const float SpeedScaleRestoreSpeedPerSecond = 1f;
    protected float SpeedScaleStep => SpeedScaleRestoreSpeedPerSecond * Time.fixedDeltaTime;
    //DIRECTION
    protected Vector2 Direction => (TargetPosition - MyPosition).normalized;
    protected Vector2 TargetPosition => target.transform.position;
    protected Vector2 MyPosition => myUnit.transform.position;
    //ACCELERATION
    protected float AccelerationSpeed => myUnit.AccelerationSpeed.Value;

    protected MovementControllerEnemy(Enemy myUnit, GameObject target)
    {
        this.myUnit = myUnit;
        this.target = target;
    }

    public abstract void FixedUpdateAccelerationStep();
    public abstract void Stag();
    public abstract void KnockBackFromTarget(Entity collisionEntity);

    public Vector2 GetMovementDirection()
    {
        return Direction;
    }
}