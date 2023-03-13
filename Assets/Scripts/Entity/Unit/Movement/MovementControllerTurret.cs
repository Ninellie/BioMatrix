using UnityEngine;
using static Math2D;

public class MovementControllerTurret
{
    private readonly GameObject _attractor;
    private readonly Unit _myUnit;
    private readonly Stat _orbitRadius;
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
    public MovementControllerTurret(Unit myUnit, GameObject attractor, Stat orbitRadius)
    {
        _myUnit = myUnit;
        _attractor = attractor;
        var r = Random.Range(0f, 360f);
        _currentAngle = r;
        _orbitRadius = orbitRadius;
    }
    public void OrbitalFixedUpdateStep()
    {
        CurrentAngle += OrbitalSpeed * Time.fixedDeltaTime;
        float fi = _currentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = _circle.GetPointOn(_orbitRadius.Value, Center, fi);
        _myUnit.Rb2D.MovePosition(nextPosition);//set new position
    }
}