using UnityEngine;

public class Enemy : Unit
{
    public GameObject onDeathDrop;
    protected override EntityStatsSettings Settings => GlobalStatsSettingsRepository.EnemyStats;
    [SerializeField] private GameObject _damagePopup;
    private GameObject _droppedDamagePopup;
    
    private Vector2 _collisionPoint;
    private GameObject _collisionGameObject;
    private Rigidbody2D _rb2D;
    protected new void OnEnable()
    {
        base.OnEnable();
    }
    protected new void OnDisable()
    {
        base.OnDisable();
    }
    protected new void Awake()
    {
        _rb2D = this.GetComponent<Rigidbody2D>();
        SetStats(Settings);
        SetMovement();
        RestoreLifePoints();
    }
    protected new void Update()
    {
        base.Update();
    }
    protected new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionGameObject = collision.gameObject;
        _collisionPoint = _collisionGameObject.transform.position;
        switch (_collisionGameObject.tag)
        {
            case "Player":
                TakeDamage(CurrentLifePoints);
                break;
            case "Projectile":
                TakeDamage(MinimalDamageTaken);
                DropDamagePopup(MinimalDamageTaken, _collisionPoint);
                break;
        }
    }
    protected override void Death()
    {
        base.Death();
        if (_collisionGameObject.tag == "Projectile")
        {
            DropBonus();
        }
    }
    protected override void SetMovement()
    {
        movement = new Movement(gameObject, MovementMode.Pursue, speed.Value);
        movement.SetPursuingTarget(GameObject.FindGameObjectsWithTag("Player")[0]);
    }
    
    private void DropBonus()
    {
        Instantiate(onDeathDrop, _rb2D.position, _rb2D.transform.rotation);
    }
    private void DropDamagePopup(int damage, Vector2 transform)
    {
        var droppedDamagePopup = Instantiate(_damagePopup);
        droppedDamagePopup.transform.position = transform;
        droppedDamagePopup.GetComponent<DamagePopup>().Setup(damage);
    }
}