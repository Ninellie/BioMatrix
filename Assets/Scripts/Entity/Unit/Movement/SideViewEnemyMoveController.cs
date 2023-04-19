using UnityEngine;

public class SideViewEnemyMoveController : EnemyMoveController
{
    public SideViewEnemyMoveController(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
        Vector2 nextPosition = MyPosition;
        if (knockbackTime > 0)
        {
            nextPosition += KnockbackVelocity * Time.fixedDeltaTime;
            knockbackTime -= Time.fixedDeltaTime;
            if (knockbackTime <= 0)
            {
                KnockbackDirection = Vector2.zero;
            }
        }
        nextPosition += MovementVelocity * Time.fixedDeltaTime;
        if (SpeedScale < 1)
        {
            SpeedScale += SpeedScaleStep;
        }
        MyUnit.Rb2D.MovePosition(nextPosition);
    }
    public override void Stag()
    {
        SpeedScale = 0;
    }
    public override void KnockBackFromTarget(float thrustPower)
    {
        knockbackTime = thrustPower / knockbackSpeed;
        KnockbackDirection = (MyPosition - TargetPosition).normalized;
        Stag();
        Debug.Log("Knockback");
    }
}