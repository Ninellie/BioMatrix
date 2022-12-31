using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

public class Boon : Unit
{
    private SpriteRenderer _spriteRenderer;
    private Rarity _rarity = new Rarity();
    private void Awake() => BaseAwake(GlobalStatsSettingsRepository.BoonStats);
    private void OnEnable() => BaseOnEnable();
    private void OnDisable() => BaseOnDisable();
    private void Update() => BaseUpdate();
    private void FixedUpdate() => Movement.FixedUpdateMove();
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
        //Movement.ChangeMode(MovementMode.Pursue);
        //Movement.SetPursuingTarget(collisionGameObject);
        //var magnetismPower = collisionGameObject.GetComponent<Player>().MagnetismPower.Value;
        //if (Speed.Value == magnetismPower) return;
        //var mod = new StatModifier(OperationType.Addition, magnetismPower);
        //Speed.AddModifier(mod);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag != "Player") return;
        if (collision.collider is not CircleCollider2D) return;
        Speed.ClearModifiersList();
        Movement.ChangeMode(MovementMode.Idle);
    }
    protected void BaseAwake(UnitStatsSettings settings)
    {
        Debug.Log($"{gameObject.name} Boon Awake");
        var movement = new Movement(gameObject, MovementMode.Idle, settings.Speed);
        base.BaseAwake(settings, movement);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rarity.Value = RarityEnum.Magic;
        //SetOutline();
    }

    public void SetOutline()
    {
        var width = 0.007f;
        var color = Color.grey;

        _spriteRenderer.material.SetFloat("_OutlineWidth", width);
        _spriteRenderer.material.SetColor("_OutlineColor", color);
    }
}
