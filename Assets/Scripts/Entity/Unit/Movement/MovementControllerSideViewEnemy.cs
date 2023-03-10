using UnityEngine;

public class MovementControllerSideViewEnemy : MovementControllerEnemy
{
    private Vector2 AccelerationStep => Direction * AccelerationSpeed * Time.fixedDeltaTime;
    public MovementControllerSideViewEnemy(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
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
}