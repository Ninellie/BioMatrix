using Assets.Scripts.Core;
using UnityEngine;

public static class MathFirearm
{
    public static Vector2 GetActualShotDirection(Vector2 direction, float maxShotDeflectionAngle)
    {
        var angleInRad = Mathf.Deg2Rad * maxShotDeflectionAngle;
        var shotDeflectionAngle = MathFirearm.Range(-angleInRad, angleInRad);
        return MathFirearm.Rotate(direction, shotDeflectionAngle);
    }

    public static Vector2 Rotate(Vector2 point, float angle)
    {
        Vector2 rotatedPoint;
        rotatedPoint.x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
        rotatedPoint.y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
        return rotatedPoint;
    }

    public static float Range(float minInclusive, float maxInclusive)
    {
        var std = PeterAcklamInverseCDF.NormInv(Random.value);
        return PeterAcklamInverseCDF.RandomGaussian(std, minInclusive, maxInclusive);
    }
}