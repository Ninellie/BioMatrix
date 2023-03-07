using UnityEngine;

public class MovementControllerPlayer
{
    private readonly Player _myUnit;
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
    private Vector2 Direction
    {
        get => _direction;
        set => _direction = value.normalized;
    }
    private Vector2 _direction;
    private Vector2 Velocity => Direction * Speed;
    public MovementControllerPlayer(Player myUnit)
    {
        _myUnit = myUnit;
    }
    public void FixedUpdateStep()
    {
        if (SpeedScale < 1f) _speedScale += SpeedScaleStep;
        _myUnit.rb2D.velocity = Velocity;
    }
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }
    public void Stag()
    {
        _speedScale = 0;
    }
    public void KnockBack(Entity collisionEntity)
    {
        if (SpeedScale < 1f) { return; }
        //Stag();
        float thrustPower = collisionEntity.KnockbackPower.Value;
        Vector2 difference = (Vector2)_myUnit.transform.position - (Vector2)collisionEntity.transform.position;
        Vector2 knockbackVelocity = difference.normalized * thrustPower * _myUnit.rb2D.mass;
        _myUnit.rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
    }
}