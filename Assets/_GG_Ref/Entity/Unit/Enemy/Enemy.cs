using System;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private GameObject _onDeathDrop;
    [SerializeField] private GameObject _damagePopup;

    private Vector2 _collisionGameObjectCentre;
    private GameObject _collisionGameObject;
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.EnemyStats);
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
    //private void Repel()
    //{
    //    if (_collisionGameObject.tag != "Enemy") return;
    //    var collisionGameObjectCircleCollider2D = _collisionGameObject.GetComponent<CircleCollider2D>();
    //    var overlap = _circleCollider2D.Distance(collisionGameObjectCircleCollider2D).isOverlapped;
    //    if (!overlap) return;
    //    var a_pointOnMyCircle = _circleCollider2D.ClosestPoint(_collisionGameObject.transform.position);
    //    var b_pointOnCollisionGameObjectCircle = collisionGameObjectCircleCollider2D.ClosestPoint(gameObject.transform.position);
    //    var c = b_pointOnCollisionGameObjectCircle - a_pointOnMyCircle;
    //    var repelVector2 = a_pointOnMyCircle - c;
    //    _rigidbody2D.MovePosition(repelVector2);
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionGameObject = collision.gameObject;
        _collisionGameObjectCentre = _collisionGameObject.transform.position;
        switch (_collisionGameObject.tag)
        {
            case "Player":
                TakeDamage(CurrentLifePoints);
                break;
            case "Projectile":
                TakeDamage(MinimalDamageTaken);
                DropDamagePopup(MinimalDamageTaken, _collisionGameObjectCentre);
                break;
            case "Enemy":
                //Repel();
                break;
        }
    }
    protected void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Enemy Awake");
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        //_level = InitialLevel;
        var movement = new Movement(gameObject, MovementMode.Seek, settings.Speed);
        movement.SetPursuingTarget(FindObjectOfType<Player>().gameObject);
        base.BaseAwake(settings, movement);
    }
    protected override void Death()
    {
        base.Death();
        if (_collisionGameObject.tag == "Projectile")
        {
            DropBonus();
        }
    }
    private void DropBonus()
    {
        Instantiate(_onDeathDrop, _rigidbody2D.position, _rigidbody2D.transform.rotation);
    }
    private void DropDamagePopup(int damage, Vector2 positionVector2)
    {
        var droppedDamagePopup = Instantiate(_damagePopup);
        droppedDamagePopup.transform.position = positionVector2;
        droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
    }
}