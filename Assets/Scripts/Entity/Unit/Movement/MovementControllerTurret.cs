using UnityEngine;

public class MovementControllerTurret
{
    private readonly GameObject _attractor;
    private readonly Unit _myUnit;
    private float OrbitRadius => _myUnit.AccelerationSpeed.Value;
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
    private float OrbitalSpeed => _myUnit.Speed.Value;//in degrees per second
    private Vector2 Center => _attractor.transform.position;
    private Vector2 MyPosition => _myUnit.transform.position;
    public MovementControllerTurret(Unit myUnit, GameObject attractor)
    {
        _myUnit = myUnit;
        _attractor = attractor;
        var r = Random.Range(0f, 360f);
        _currentAngle = r;
    }
    public void OrbitalFixedUpdateStep()
    {
        CurrentAngle += OrbitalSpeed * Time.fixedDeltaTime;
        float fi = _currentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = _circle.GetPointOn(OrbitRadius, Center, fi);
        _myUnit.Rb2D.MovePosition(nextPosition);//set new position
    }
}