using UnityEngine;

public class SideViewEnemyMoveController : EnemyMoveController
{
    private Vector2 AccelerationStep => Direction * AccelerationSpeed * Time.fixedDeltaTime;
    public SideViewEnemyMoveController(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
        //if (SpeedScale == 0)
        if (SpeedScale < 0.1f)
        {
            if (SpeedScale < 1f) SpeedScale += SpeedScaleStep;
            return;
        }
        else
        {
            Velocity += AccelerationStep;
            MyUnit.Rb2D.velocity = Velocity;
            if (SpeedScale < 1f) SpeedScale += SpeedScaleStep;
        }
    }
    public override void Stag()
    {
        SpeedScale = 0;
    }
    public override void KnockBackFromTarget(float thrustPower)
    {
        if (SpeedScale < 1f) { return; }
        Stag();
        Vector2 difference = (MyPosition - TargetPosition).normalized;
        Vector2 knockbackVelocity = difference * thrustPower * MyUnit.Rb2D.mass;
        MyUnit.Rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
    }
}