using UnityEngine;


public class AboveViewEnemyMoveController : EnemyMoveController
{
    private Vector2 ViewDirection => MyUnit.transform.up;
    //ACCELERATION
    private Vector2 AccelerationStep => ViewDirection * AccelerationSpeed * Time.fixedDeltaTime;
    //ROTATION
    private float RotationSpeed => MyUnit.RotationSpeed.Value;
    private float RotationStep => RotationSpeed * Time.fixedDeltaTime;
    public AboveViewEnemyMoveController(Enemy myUnit, GameObject target) : base(myUnit, target)
    {
    }
    public override void FixedUpdateAccelerationStep()
    {
        TurnToTargetStep();
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
    public void TurnToTarget()
    {
        var directionAngle = GetDirectionAngle(Direction);
        MyUnit.Rb2D.rotation = directionAngle;
    }
    private void TurnToTargetStep()
    {
        var angle = GetDirectionAngle(Direction);
        var speed = RotationStep;
        var lerpAngle = Mathf.LerpAngle(MyUnit.Rb2D.rotation, angle, speed);
        MyUnit.Rb2D.rotation = lerpAngle;
    }
    private float GetDirectionAngle(Vector2 direction)
    {
        var angleInDegrees = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
        return angleInDegrees;
    }
}