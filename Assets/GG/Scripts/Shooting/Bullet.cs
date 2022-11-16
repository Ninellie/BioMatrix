//using UnityEngine;

//public class Bullet : Ammo
//{
//    public override void Launch(Vector2 direction, FirearmSettings firearmSettings)
//    {
//        Vector2 actualShotDirection = GetActualShotDirection(direction, firearmSettings.MaxShotDeflectionAngle);
//        gameObject.AddComponent<Movement>();
//        gameObject.GetComponent<Movement>().ChangeMode(MovementMode.Rectilinear);
//        gameObject.GetComponent<Movement>().AccelerateInDirection(firearmSettings.ShootForce, actualShotDirection);
//    }
//    private Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
//    {
//        float angleInRad = (float)Mathf.Deg2Rad * maxShotDeflectionAngle;
//        float shotDeflectionAngle = Lib2DMethods.Range(-angleInRad, angleInRad);
//        return Lib2DMethods.Rotate(direction, shotDeflectionAngle);
//    }
//}