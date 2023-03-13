using UnityEngine;

public class MovementControllerBullet
{
    private readonly Projectile _myUnit;
    private Vector2 Velocity => Direction * Speed;
    private float Speed => _myUnit.Speed.Value;
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
    }
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }
    public void SetVelocity(Vector2 velocity)
    {
        _myUnit.Rb2D.velocity = velocity;
    }
}