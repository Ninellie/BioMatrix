using UnityEngine;

public class Circle
{
    public float GetRadiusInscribedAroundTheCamera()
    {
        var camHeight = Camera.main.orthographicSize * 2;
        var camWidth = camHeight * Camera.main.aspect;
        return GetHypotenuseLength(camHeight, camWidth) / 2;
    }
    public Vector2 GetPointOn(float radius, Vector2 circleCenter, float fi)
    {
        var randomPointOnBaseCircle = new Vector2(Mathf.Cos(fi) * radius, Mathf.Sin(fi) * radius);
        var randomPointOnActualCircle = circleCenter + randomPointOnBaseCircle;
        return randomPointOnActualCircle;
    }
    public float GetRandomAngle()
    {
        return Random.Range(0, Mathf.PI * 2);
    }
    private float GetHypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }
}