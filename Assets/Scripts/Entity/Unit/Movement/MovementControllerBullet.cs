using UnityEngine;

public class MovementControllerBullet
{
    private readonly Projectile _myUnit;
    private Vector2 Velocity => Direction * Speed * SpeedScale;
    private float Speed => _myUnit.Speed.Value;

    private float SpeedScale
    {
        get => _speedScale;
        set
        {
            _speedScale = value switch
            {
                < 0 => 0,
                > 1 => 1,
                _ => value
            };
        }
    }
    private float _speedScale = 1f;
    private float SpeedDecreaseStep => SpeedDecreasePerSecond * Time.fixedDeltaTime;
    private float SpeedDecreasePerSecond => 1 / TimeToStop;
    private float TimeToStop => _myUnit.timeToStop;
    private Vector2 Direction
    {
        get => _direction;
        set => _direction = value.normalized;
    }
    private Vector2 _direction;
    public MovementControllerBullet(Projectile myUnit)
    {
        _myUnit = myUnit;
    }
    public void FixedUpdateStep()
    {
        _myUnit.Rb2D.velocity = Velocity;
        SpeedScale -= SpeedDecreaseStep;
    }
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }
}