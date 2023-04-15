using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SideViewEnemyMoveController : EnemyMoveController
{
    public float knockbackPower;
    public float knockbackSpeed = 700;
    public float knockbackTime = 0;
    private Vector2 AccelerationStep => Direction * AccelerationSpeed * Time.fixedDeltaTime;
    public SideViewEnemyMoveController(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
        Vector2 nextPosition = MyPosition;
        if (knockbackTime > 0)
        {
            Vector2 difference = (MyPosition - TargetPosition).normalized;
            Vector2 addedPosition = difference * knockbackSpeed * Time.fixedDeltaTime;
            nextPosition += addedPosition;
            knockbackTime -= Time.fixedDeltaTime;
        }

        Vector2 movement = Direction * Speed * Time.fixedDeltaTime;

        nextPosition += movement;
        if (SpeedScale < 1)
        {
            SpeedScale += SpeedScaleStep;
        }

        MyUnit.Rb2D.MovePosition(nextPosition);
    }
    public override void KnockBackFromTarget(float thrustPower)
    {
        knockbackPower = thrustPower;
        knockbackTime = thrustPower / knockbackSpeed;
        Stag();
        Debug.Log("Knockback");
    }
    public override void Stag()
    {
        SpeedScale = 0;
    }
}