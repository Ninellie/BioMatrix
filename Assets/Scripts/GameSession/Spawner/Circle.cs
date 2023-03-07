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
        var pointOnBaseCircle = new Vector2(Mathf.Cos(fi) * radius, Mathf.Sin(fi) * radius);
        var pointOnActualCircle = circleCenter + pointOnBaseCircle;
        return pointOnActualCircle;
    }
    /// <summary>
    /// Moves position, along the circle, with center in circleCentre and radius radius, by angular speed and interval.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="circleCentre"></param>
    /// <param name="radius"></param>
    /// <param name="angularSpeed">In radians</param>
    /// <param name="timeInterval">In seconds</param>
    /// <returns>Vector2</returns>
    public Vector2 GetNextPointOn(Vector2 position, Vector2 circleCentre, float radius, float angularSpeed, float timeInterval)
    {
        var currentAngle = Vector2.SignedAngle(circleCentre, position);
        var angleStep = angularSpeed * timeInterval;
        var nextAngle = currentAngle + angleStep;
        var nextPosition = this.GetPointOn(radius, circleCentre, nextAngle);
        return nextPosition;
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