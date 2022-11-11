using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameObject _pursuingTarget;
    [SerializeField] private int _movementMode;
    //private bool IsRectilinear => _movementMode == 0;
    //private bool IsPursuing => _movementMode == 1;
    //private bool IsSeeking => _movementMode == 2;
    private GameObject DrivenGameObject => gameObject;
    private Rigidbody2D DrivenRigidbody2D => DrivenGameObject.GetComponent<Rigidbody2D>();
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _movementDirection;
    private Vector2 Velocity => _movementDirection.normalized * _speed;
    private void Awake()
    {
        _speed = 0;
        _movementDirection = Vector2.zero.normalized;
    }

    private void FixedUpdate()
    {
        switch (_movementMode)
        {
            case 0:
                Move();
                break;
            case 1:
                SetMovementDirection(_pursuingTarget.GetComponent<Rigidbody2D>().position - DrivenRigidbody2D.position);
                Move();
                break;
            case 2:
                LookToPursuingTarget();
                SetMovementDirection(_pursuingTarget.GetComponent<Rigidbody2D>().position - DrivenRigidbody2D.position);
                Move();
                break;
        }
    }
    private void Move()
    {
        DrivenRigidbody2D.MovePosition(DrivenRigidbody2D.position + Velocity * Time.fixedDeltaTime);
    }

    public void ChangeMode(int i)
    {
        _movementMode = i;
    }

    public void SetPursuingTarget(GameObject pursuingTarget)
    {
        _pursuingTarget = pursuingTarget;
    }
    public void AccelerateInDirection(float speed, Vector2 direction)
    {
        ChangeMovementDirection(direction);
        Accelerate(speed);
    }

    public void Accelerate(float speed)
    {
        if (speed >= 0)
        {
            _speed += speed;
        }
        else
        {
            return;
        }
    }

    public void SlowDown(float speed)
    {
        if (speed >= 0)
        {
            _speed -= speed;
        }
        else
        {
            return;
        }
    }

    public void ChangeMovementDirection(Vector2 direction)
    {
        _movementDirection += direction;
    }

    public void SetMovementDirection(Vector2 direction)
    {
        _movementDirection = direction;
    }

    private void LookToPursuingTarget()
    {
        float angle = (Mathf.Atan2(DirectionToPursuingTarget().y, DirectionToPursuingTarget().x) - Mathf.PI / 2) * Mathf.Rad2Deg;

        DrivenRigidbody2D.rotation = angle;
    }
    private Vector2 DirectionToPursuingTarget()
    {
        var horizontal = _pursuingTarget.transform.position.x - DrivenRigidbody2D.transform.position.x;
        var vertical = _pursuingTarget.transform.position.y - DrivenRigidbody2D.transform.position.y;

        return new Vector2(horizontal, vertical);
    }

}
