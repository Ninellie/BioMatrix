using UnityEngine;

public class SideViewEnemyMoveController : EnemyMoveController
{
    private Vector2 AccelerationStep => Direction * AccelerationSpeed * Time.fixedDeltaTime;
    public SideViewEnemyMoveController(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
        if (SpeedScale < 0.05)
        {
            SpeedScale += SpeedScaleStep;
            return;
        }
        Velocity += AccelerationStep;
        MyUnit.Rb2D.velocity = Velocity;
        if (SpeedScale < 1f) SpeedScale += SpeedScaleStep;
    }
    //public override void FixedUpdateAccelerationStep()
    //{
    //    if (SpeedScale < 1f)
    //    {
    //        SpeedScale += SpeedScaleStep;
    //        return;
    //    }
    //    Velocity += AccelerationStep;
    //    MyUnit.Rb2D.velocity = Velocity;
    //}
    public override void Stag()
    {
        SpeedScale = 0;
    }
    public override void KnockBackFromTarget(float thrustPower)
    {
        if (SpeedScale < 1f) { return; }
        Debug.LogWarning($"{MyUnit.name} knocking STARTS from {Target.name} WITH THRUSTPOWER: {thrustPower}");
        Stag();
        Vector2 difference = (MyPosition - TargetPosition).normalized;
        Vector2 knockbackVelocity = difference * thrustPower * MyUnit.Rb2D.mass;
        MyUnit.Rb2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
        Debug.LogWarning($"{MyUnit.name} knocking END from {Target.name}");
    }
}