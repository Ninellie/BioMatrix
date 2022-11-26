using UnityEngine;

public class Projectile : Unit
{
    protected new bool IsOnScreen
    {
        get => IsOnScreen;
        private set
        {
            switch (value)
            {
                case false:
                    IsOnScreen = false;
                    TakeDamage(CurrentLifePoints);
                    break;
                case true:
                    IsOnScreen = true;
                    break;
            }
        }
    }

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
    public virtual void Launch(Vector2 direction, float force)
    {
        movement.ChangeMode(MovementMode.Rectilinear);
        movement.AccelerateInDirection(force, direction);
    }
    public void Launch(Vector2 direction, float maxShotDeflectionAngle, float force)
    {
        var actualShotDirection = GetActualShotDirection(direction, maxShotDeflectionAngle);
        Launch(actualShotDirection, force);
    }
    private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
    {
        var angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
        var shotDeflectionAngle = Lib2DMethods.Range(-angleInRad, angleInRad);
        return Lib2DMethods.Rotate(direction, shotDeflectionAngle);
    }
}