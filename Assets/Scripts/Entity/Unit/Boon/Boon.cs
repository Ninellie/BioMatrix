using UnityEngine;

public class Boon : Unit
{
    public float speed = 1500;
    public UnitStatsSettings Settings => GetComponent<UnitStatsSettings>();
    private readonly Rarity _rarity = new Rarity();
    private void Awake() => BaseAwake(Settings);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void OnTriggerStay2D(Collider2D otherCollider2D)
    {
        if (!otherCollider2D.gameObject.CompareTag("Player")) return;
        Vector2 nextPosition = transform.position;
        Vector2 direction = (otherCollider2D.transform.position - transform.position).normalized;
        Vector2 movementVelocity = direction * speed;
        nextPosition += movementVelocity * Time.fixedDeltaTime;
        
        Rb2D.MovePosition(nextPosition);
    }
    private void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        var collisionGameObject = otherCollider2D.gameObject;

        if (!otherCollider2D.gameObject.CompareTag("Player")) return;
        if (otherCollider2D is not BoxCollider2D) return;
        Debug.Log("The exp crystal was taken");
        TakeDamage(LifePoints.GetValue());
        collisionGameObject.GetComponent<Player>().Experience++;
    }
    protected override void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Boon Awake");
        statFactory = Camera.main.GetComponent<StatFactory>();
        base.BaseAwake(settings);
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rarity.Value = RarityEnum.Magic;
    }
}
