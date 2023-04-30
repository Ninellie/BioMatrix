using UnityEngine;

public class MovementControllerTurret
{
    private readonly Turret _myTurret;
    private float OrbitRadius => _myTurret.AccelerationSpeed.Value;
    private readonly Circle _circle = new Circle();
    private float _currentAngle;
    private float CurrentAngle
    {
        get => _currentAngle;
        set
        {
            _currentAngle = value switch
            {
                > 360f => value - 360f,
                _ => value
            };
        }
    }
    private float OrbitalSpeed => _myTurret.Speed.Value;//in degrees per second
    private Vector2 Center => _myTurret.GetAttractor().transform.position;
    public MovementControllerTurret(Turret myTurret)
    {
        _myTurret = myTurret;
        var r = Random.Range(0f, 360f);
        _currentAngle = r;
    }
    public void OrbitalFixedUpdateStep()
    {
        CurrentAngle += OrbitalSpeed * Time.fixedDeltaTime;
        float fi = CurrentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = _circle.GetPointOn(OrbitRadius, Center, fi);
        _myTurret.Rb2D.MovePosition(nextPosition);//set new position
    }
}