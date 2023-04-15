using UnityEngine;


public class AboveViewEnemyMoveController : EnemyMoveController
{
    public float knockbackPower;
    public float knockbackSpeed = 700;
    public float knockbackTime = 0;
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

        Vector2 nextPosition = MyPosition;
        if (knockbackTime > 0)
        {
            Vector2 difference = (MyPosition - TargetPosition).normalized;
            Vector2 addedPosition = difference * knockbackSpeed * Time.fixedDeltaTime;
            nextPosition += addedPosition;
            knockbackTime -= Time.fixedDeltaTime;
        }

        Vector2 moveStep = ViewDirection * Speed * Time.fixedDeltaTime;

        nextPosition += moveStep;

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
        knockbackPower = thrustPower;
        knockbackTime = thrustPower / knockbackSpeed;
        Stag();
        Debug.Log("Knockback");
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