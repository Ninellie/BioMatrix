using UnityEngine;

public class SideViewEnemyMoveController : EnemyMoveController
{
    private Vector2 AccelerationStep => Direction * AccelerationSpeed * Time.fixedDeltaTime;
    public SideViewEnemyMoveController(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
        Velocity += AccelerationStep;
        MyUnit.Rb2D.velocity = Velocity;
        if (SpeedScale < 1f) SpeedScale += SpeedScaleStep;
    }
    public override void Stag()
    {
        SpeedScale = 0;
    }
    public override void KnockBackFromTarget(Entity collisionEntity)
    {
        if (SpeedScale < 1f) { return; }
        Stag();
        float thrustPower = collisionEntity.KnockbackPower.Value;
        Vector2 difference = (MyPosition - TargetPosition).normalized;
        Vector2 knockbackVelocity = difference * thrustPower * MyUnit.Rb2D.mass;
        MyUnit.Rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
    }
}