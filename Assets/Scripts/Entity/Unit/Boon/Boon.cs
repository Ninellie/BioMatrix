using UnityEngine;
using Color = UnityEngine.Color;

public class Boon : Unit
{
    //private SpriteRenderer _spriteRenderer;
    private readonly Rarity _rarity = new Rarity();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.BoonStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        var collisionGameObject = collider2D.gameObject;

        if (collisionGameObject.tag != "Player") return;
        if (collider2D is not BoxCollider2D) return;
        Debug.Log("The exp crystal was taken");
        TakeDamage(CurrentLifePoints);
        collisionGameObject.GetComponent<Player>().Experience++;
        Debug.Log("The exp crystal pursue player");
    }
    protected override void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Boon Awake");
        base.BaseAwake(settings);
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rarity.Value = RarityEnum.Magic;
    }
}
