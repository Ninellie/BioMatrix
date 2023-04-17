using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementControllerPlayer
{
    private readonly Player _myUnit;
    private readonly float _knockbackSpeed = 1500;
    private float _knockbackPower = 0;
    private float _knockbackTime = 0;
    private Vector2 _knockbackPosition = Vector2.zero;
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
    private Vector2 Direction
    {
        get => _direction;
        set => _direction = value.normalized;
    }
    private Vector2 _direction;
    //private Vector2 Velocity => Direction * Speed;
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
        }

        Vector2 movementStep = Direction * Speed * Time.fixedDeltaTime;

        nextPosition += movementStep;

        if (SpeedScale < 1)
        {
            SpeedScale += SpeedScaleStep;
        }

        _myUnit.Rb2D.MovePosition(nextPosition);
    }
    //public void FixedUpdateStep()
    //{
    //    if (SpeedScale < 1f) _speedScale += SpeedScaleStep;
    //    _myUnit.Rb2D.velocity = Velocity;
    //}
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
        float thrustPower = collisionEntity.KnockbackPower.Value;
        _knockbackPosition = collisionEntity.transform.position;
        _knockbackPower = thrustPower;
        _knockbackTime = thrustPower / _knockbackSpeed;
        Debug.Log("Knockback");
    }
    //public void KnockBack(Entity collisionEntity)
    //{
    //    if (SpeedScale < 1f) { return; }
    //    //Stag();
    //    float thrustPower = collisionEntity.KnockbackPower.Value;
    //    Vector2 difference = (Vector2)_myUnit.transform.position - (Vector2)collisionEntity.transform.position;
    //    Vector2 knockbackVelocity = difference.normalized * thrustPower * _myUnit.Rb2D.mass;
    //    _myUnit.Rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
    //}
}