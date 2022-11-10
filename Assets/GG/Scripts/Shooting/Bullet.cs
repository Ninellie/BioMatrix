using UnityEngine;

public class Bullet : Ammo
{
    public override void Launch(Vector2 direction, FirearmSettings firearmSettings)
    {
        Vector2 actualShotDirection = GetActualShotDirection(direction, firearmSettings.MaxShotDeflectionAngle);
        AccelerateProjectileInDirection(gameObject, firearmSettings.ShootForce, actualShotDirection);
    }
    private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
    {
        float angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
        float shotDeflectionAngle = Lib2DMethods.Range(-angleInRad, angleInRad);
        return Lib2DMethods.Rotate(direction, shotDeflectionAngle);
    }
    private void AccelerateProjectileInDirection(GameObject projectile, float force, Vector2 direction)
    {
        Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
}