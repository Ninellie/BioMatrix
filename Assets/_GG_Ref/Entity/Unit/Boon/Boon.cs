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
        Debug.Log("The Boon was taken");
        var collisionGameObject = collision.gameObject;
        switch (collisionGameObject.tag)
        {
            case "Player":
                TakeDamage(CurrentLifePoints);
                break;
        }
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
