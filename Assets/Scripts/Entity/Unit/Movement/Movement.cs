using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.Entity.Unit.Movement
{
    public class Movement
    {
        public float Speed { get; private set; }
        public float TurningSpeed { get; private set; }
        private Rigidbody2D _pursuingRigidbody2D;
        private readonly Unit _drivenUnit;
        private MovementState _currentState;
        private Vector2 _movementDirection;
        private readonly Rigidbody2D _drivenRigidbody2D;
        private float _velocityScale = 1f;
        private const float VelocityScaleStep = 0.05f;
        private const float AccelerationSpeed = 200f;
        private float VelocityScale
        {
            get => _velocityScale;
            set
            {
                if (value >= 1f)
                {
                    if (_isStagger) 
                    {
                        _isStagger = false;
                    }
                    _velocityScale = 1f;
                }
                if (value <= 0)
                {
                    _velocityScale = 0;
                }
                else
                {
                    _velocityScale = value;
                }
            }
        }

        private bool _isStagger = false;

        private Vector2 Velocity => _movementDirection.normalized * Speed * VelocityScale;
        private Vector2 LocalUp => _drivenRigidbody2D.transform.up;
        public Movement(Unit drivenUnit) : this(drivenUnit, MovementState.Idle, 0)
        {
        }
        public Movement(Unit drivenUnit, float speed) : this(drivenUnit, MovementState.Idle, speed)
        {
        }
        public Movement(Unit drivenUnit, MovementState currentState, float speed)
        {
            _drivenUnit = drivenUnit;
            _currentState = currentState;
            Speed = speed;
            TurningSpeed = 5f;
            _movementDirection = Vector2.zero.normalized;
            _pursuingRigidbody2D = null;
            _drivenRigidbody2D = drivenUnit.GetComponent<Rigidbody2D>();
        }
        public void FixedUpdateMove()
        {
            Move();
        }
        private void Move()
        {
            switch (_currentState)
            {
                case MovementState.Idle:
                    //DrivenRigidbody2D.velocity = Vector2.zero;
                    break;
                case MovementState.Rectilinear:
                    if (_isStagger)
                    {
                        VelocityScale += VelocityScaleStep;
                        Force(ForceMode2D.Impulse);
                    }
                    else
                    {
                        SetVelocity();
                    }
                    break;
                case MovementState.Pursue:
                    if (_isStagger) VelocityScale += VelocityScaleStep;
                    Pursue();
                    break;
                case MovementState.Seek:
                    if (_isStagger) VelocityScale += VelocityScaleStep;
                    SeekAndPursue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Force(ForceMode2D forceMode2D)
        {
            if (_drivenRigidbody2D.velocity.magnitude.Equals(Velocity.magnitude)) return;
            var difference = Velocity - _drivenRigidbody2D.velocity;
            var forcePower = difference.normalized * AccelerationSpeed * _drivenRigidbody2D.mass;
            _drivenRigidbody2D.AddForce(forcePower, forceMode2D);
        }
        public void ChangeState(MovementState state)
        {
            _currentState = state;
            if (state == MovementState.Idle)
            {
                _drivenRigidbody2D.velocity = Vector2.zero;
            }
        }
        public void SetPursuingTarget(GameObject pursuingTarget)
        {
            _pursuingRigidbody2D = pursuingTarget.GetComponent<Rigidbody2D>();
        }
        public void Accelerate(float speed)
        {
            if (speed < 0) return;
            Speed += speed;
        }
        public void SlowDown(float speed)
        {
            if (speed < 0) return;
            var newSpeed = Speed - speed;
            if (newSpeed <= 0) { Speed = 0; }
            else { Speed -= speed; }
        }
        public void SetMovementDirection(Vector2 direction)
        {
            _movementDirection = direction.normalized;
        }
        public void KnockBack(Entity collisionEntity)
        {
            if (_isStagger)
            {
                return;
            }
            Stag();
            float thrustPower = collisionEntity.KnockbackPower.Value;
            Vector2 difference = (Vector2)_drivenRigidbody2D.transform.position - (Vector2)collisionEntity.transform.position;
            Vector2 knockbackVelocity = difference.normalized * thrustPower * _drivenRigidbody2D.mass;
            _drivenRigidbody2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
        }
        public void KnockBackFromPlayer(Entity collisionEntity, Vector2 playerPosition)
        {
            if (_isStagger)
            {
                return;
            }
            Stag();
            float thrustPower = collisionEntity.KnockbackPower.Value;
            Vector2 difference = (Vector2)_drivenRigidbody2D.transform.position - playerPosition;
            Vector2 knockbackVelocity = difference.normalized * thrustPower * _drivenRigidbody2D.mass;
            _drivenRigidbody2D.AddForce(knockbackVelocity, ForceMode2D.Impulse);
        }
        private void Stag()
        {
            VelocityScale = 0;
            _isStagger = true;
        }
        public void SetVelocity()
        {
            _drivenRigidbody2D.velocity = Velocity;
        }
        private void Pursue()
        {
            if (_pursuingRigidbody2D == null) return;
            SetMovementDirection(_pursuingRigidbody2D.position - _drivenRigidbody2D.position);
            //SetVelocity();
            Force(ForceMode2D.Force);
        }
        private void SeekAndPursue()
        {
            if (_pursuingRigidbody2D == null) return;
            TurnToPursuingTarget();
            SetMovementDirection(LocalUp);
            //SetVelocity();
            Force(ForceMode2D.Force);
        }
        private void TurnToPursuingTarget()
        {
            var angle = GetAngleToPursuingTarget();
            var speed = TurningSpeed * Time.fixedDeltaTime;
            var lerpAngle = Mathf.LerpAngle(_drivenRigidbody2D.rotation, angle, speed);
            _drivenRigidbody2D.rotation = lerpAngle;
        }
        private float GetAngleToPursuingTarget()
        {
            var direction = GetDirectionToPursuingTarget();
            var angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2) * Mathf.Rad2Deg;
            return angle;
        }
        private Vector2 GetDirectionToPursuingTarget()
        {
            if (_pursuingRigidbody2D == null) return new Vector2(0, 0);
            var vec = (Vector2)_pursuingRigidbody2D.transform.position - _drivenRigidbody2D.position;
            return vec;
        }
    }
}