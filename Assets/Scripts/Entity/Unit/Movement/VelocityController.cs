using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VelocityController
{
    private Vector2 Velocity
    {
        get => _velocity;
        set
        {
            if (value.sqrMagnitude > SpeedMax * SpeedMax)
            {
                _velocity = value.normalized * SpeedMax;
            }

        }
    }
    private Vector2 Acceleration => SpeedMax * MovementDirection * Time.fixedDeltaTime;
    private float Restitution
    {
        get => _restitution;
        set
        {
            switch (value)
            {
                case >= 1:
                    _restitution = 1f;
                    return;
                case <= 0:
                    _restitution = 0f;
                    return;
                default:
                    _restitution = value;
                    break;
            }
        }
    }
    private float InvMass => Mass == 0 ? 0 : 1f / Mass;
    private const float Mass = 1.0f;
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
    private float _restitution = 1.0f;
    private readonly Unit _myUnit;
    private float SpeedMax => _myUnit.Speed.Value * SpeedScale;
    private Vector2 _velocity;
    private Vector2 MovementDirection
    {
        get => _target == null ? _direction : (TargetPosition - MyPosition).normalized;
        set => _direction = value.normalized;
    }

    private Vector2 _direction;
    private GameObject _target;
    private Vector2 TargetPosition => _target == null ? Vector2.zero : _target.transform.position;

    private Vector2 MyPosition => _myUnit.transform.position;

    public VelocityController(Unit myUnit)
    {
        _myUnit = myUnit;
    }
    public void ResolveCollision(VelocityController other, ContactPoint2D contactPoint2D)
    {
        Vector2 rv = other.Velocity - Velocity;

        float velAlongNormal = Vector2.Dot(rv, contactPoint2D.normal);

        if (velAlongNormal > 0) { return; }

        float e = MathF.Min(Restitution, other.Restitution);

        float j = -(1 + e) * velAlongNormal;
        j /= InvMass + other.InvMass;

        Vector2 impulse = j * contactPoint2D.normal;

        float massSum = Mass + Mass;

        float ratio = Mass / massSum;
        Velocity -= ratio * impulse;
        PositionalCorrection(other, contactPoint2D);
    }
    private void PositionalCorrection(VelocityController other, ContactPoint2D contactPoint2D)
    {
        float penetration = contactPoint2D.separation;
        const float percent = 0.2f; // 20% - 80%
        const float slop = 0.01f; // 0.01 - 0.1
        var correction = MathF.Max(penetration - slop, 0.0f) /
            (InvMass + other.InvMass) * percent * contactPoint2D.normal;
        Vector3 aPos = (Vector2)_myUnit.transform.position - InvMass * correction;
        _myUnit.transform.position = aPos;
    }
    //public void ResolveCollision(VelocityController A, VelocityController B, ContactPoint2D contactPoint2D)
    //{
    //    Vector2 rv = B.Velocity - A.Velocity;

    //    float velAlongNormal = Vector2.Dot(rv, contactPoint2D.normal);

    //    if (velAlongNormal > 0) { return; }

    //    float e = MathF.Min(A.Restitution, B.Restitution);

    //    float j = -(1 + e) * velAlongNormal;
    //    j /= A.InvMass + B.InvMass;

    //    Vector2 impulse = j * contactPoint2D.normal;

    //    float massSum = Mass + Mass;

    //    float ratio = Mass / massSum;
    //    A.Velocity -= ratio * impulse;

    //    ratio = Mass / massSum;
    //    B.Velocity += ratio * impulse;
    //}
    //private void PositionalCorrection(VelocityController A, VelocityController B, ContactPoint2D contactPoint2D)
    //{
    //    float penetration = contactPoint2D.separation;
    //    const float percent = 0.2f; // 20% - 80%
    //    const float slop = 0.01f; // 0.01 - 0.1
    //    var correction = MathF.Max(penetration - slop, 0.0f) / (A.InvMass + B.InvMass) * percent * contactPoint2D.normal;
    //    Vector3 Apos = (Vector2)A.collider.transform.position - A.InvMass * correction;
    //    Vector3 Bpos = (Vector2)B.collider.transform.position + B.InvMass * correction;
    //    A.collider.transform.position = Apos;
    //    B.collider.transform.position = Bpos;
    //}
    public void SetDirection(Vector2 direction)
    {
        MovementDirection = direction;
    }
    public void SetTarget(GameObject target)
    {
        _target = target;
    }
    public Vector2 GetFixedUpdateStep()
    {
        Velocity += Acceleration;
        _myUnit.GetComponent<Rigidbody2D>().velocity = Velocity;
        return Velocity;
    }
    public void FixedUpdateStep()
    {
        Velocity += Acceleration;
        _myUnit.GetComponent<Rigidbody2D>().velocity = Velocity;
    }
}