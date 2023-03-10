using UnityEngine;


public class MovementControllerAboveViewEnemy : MovementControllerEnemy
{
    private Vector2 ViewDirection => myUnit.transform.up;
    //ACCELERATION
    private Vector2 AccelerationStep => ViewDirection * AccelerationSpeed * Time.fixedDeltaTime;
    //ROTATION
    private float RotationSpeed => myUnit.RotationSpeed.Value;
    private float RotationStep => RotationSpeed * Time.fixedDeltaTime;
    public MovementControllerAboveViewEnemy(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }

    public override void FixedUpdateAccelerationStep()
    {
        TurnToTargetStep();
        Velocity += AccelerationStep;
        myUnit.rb2D.velocity = Velocity;
        if (SpeedScale < 1f) speedScale += SpeedScaleStep;
    }
    public override void Stag()
    {
        speedScale = 0;
    }
    public override void KnockBackFromTarget(Entity collisionEntity)
    {
        if (SpeedScale < 1f) { return; }
        Stag();
        float thrustPower = collisionEntity.KnockbackPower.Value;
        Vector2 difference = (MyPosition - TargetPosition).normalized;
        Vector2 knockbackVelocity = difference * thrustPower * myUnit.rb2D.mass;
        myUnit.rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
    }
    public void TurnToTarget()
    {
        var directionAngle = GetDirectionAngle(Direction);
        myUnit.rb2D.rotation = directionAngle;
    }
    private void TurnToTargetStep()
    {
        var angle = GetDirectionAngle(Direction);
        var speed = RotationStep;
        var lerpAngle = Mathf.LerpAngle(myUnit.rb2D.rotation, angle, speed);
        myUnit.rb2D.rotation = lerpAngle;
    }
    private float GetDirectionAngle(Vector2 direction)
    {
        var angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        return angleInDegrees;
    }
}