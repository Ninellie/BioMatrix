using System;
using System.Numerics;

public class Math2D
{
    public struct AABB
    {
        public Vector2 min;
        public Vector2 max;

        public AABB(Vector2 max, Vector2 min)
        {
            this.max = max;
            this.min = min;
        }
    }
    private bool AABBvsAABB(AABB a, AABB b)
    {
        if (a.min.X < b.min.X || a.min.X > b.max.X) return false;
        if (a.max.Y < b.min.Y || a.min.Y > b.max.Y) return false;
        return true;
    }
    public struct Circle
    {
        public float radius;
        public Vector2 position;
        public Circle(float radius, Vector2 position)
        {
            this.radius = radius;
            this.position = position;
        }
    }
    public bool CircleVsCircle(Circle a, Circle b)
    {
        var r = a.radius + b.radius;
        r *= r;
        var xPos = a.position.X + b.position.X;
        xPos *= xPos;
        var yPos = a.position.Y + b.position.Y;
        yPos *= yPos;
        return r < xPos + yPos;
    }

    public struct PhysicalEntity
    {
        public Vector2 velocity;
        public float restitution;
        public float mass;

    }

    public void ResolveCollision(PhysicalEntity a, PhysicalEntity b)
    {
        Vector2 rv = b.velocity - a.velocity;
    }
}