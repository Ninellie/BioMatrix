using UnityEngine;

public class Projectile : Unit
{
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.ProjectileStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Enemy":
                if (collisionGameObject.GetComponent<Entity>().Alive)
                {
                    TakeDamage(MinimalDamageTaken);
                }
                break;
        }
    }
    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (IsOnScreen == false)
        {
            TakeDamage(CurrentLifePoints);
        }
    }
    public virtual void Launch(Vector2 direction, float force)
    {
        Movement.ChangeMode(MovementMode.Rectilinear);
        Movement.AccelerateInDirection(force, direction);
    }
    public void Launch(Vector2 direction, float maxShotDeflectionAngle, float force)
    {
        var actualShotDirection = GetActualShotDirection(direction, maxShotDeflectionAngle);
        Launch(actualShotDirection, force);
    }
    private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
    {
        var angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
        var shotDeflectionAngle = Range(-angleInRad, angleInRad);
        return Rotate(direction, shotDeflectionAngle);
    }
    private float Range(float minInclusive, float maxInclusive)
    {
        var std = PeterAcklamInverseCDF.NormInv(Random.value);
        return PeterAcklamInverseCDF.RandomGaussian(std, minInclusive, maxInclusive);
    }
    private Vector2 Rotate(Vector2 point, float angle)
    {
        Vector2 rotatedPoint;
        rotatedPoint.x = point.x * Mathf.Cos(angle)
                          - point.y * Mathf.Sin(angle);
        rotatedPoint.y = point.x * Mathf.Sin(angle)
                          + point.y * Mathf.Cos(angle);
        return rotatedPoint;
    }
}