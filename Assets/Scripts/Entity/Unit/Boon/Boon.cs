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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag != "Player") return;
        if (collision.collider is BoxCollider2D)
        {
            Debug.Log("The exp crystal was taken");
            TakeDamage(CurrentLifePoints);
            collisionGameObject.GetComponent<Player>().Experience++;
            return;
        }
        Debug.Log("The exp crystal pursue player");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag != "Player") return;
        if (collision.collider is not CircleCollider2D) return;
        Speed.ClearModifiersList();
        //Movement.ChangeState(MovementState.Idle);
    }
    protected override void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Boon Awake");
        base.BaseAwake(settings);
        spriteRenderer = GetComponent<SpriteRenderer>();
        _rarity.Value = RarityEnum.Magic;
    }
    public void SetOutline()
    {
        var width = 0.007f;
        var color = Color.grey;
        spriteRenderer.material.SetFloat("_OutlineWidth", width);
        spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
