using System;
using System.Collections.Generic;

[Serializable]
public static class EffectRepository
{
    public static readonly Dictionary<string, IEffect> StatModEffects = new()
    {
        {
            "Gun card effect 1",
            new AddModOnAttach(
                "Gun card effect 1",
                "+50% projectile damage multiplier, +2 magazine amount",
                "Player",
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Multiplication, 50f), "ProjectileDamage"),
                    (new StatModifier(OperationType.Addition, 2f), "MagazineAmount"),
                }
            )
        },
        {
            "Secondary Gun card effect 2",
            new AddModOnAttach(
                "Secondary Gun card effect 2",
                "+50% firerate multiplier for 2 sec",
                "Player",
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Multiplication, 50f), "Firerate"),
                },
                true,
                new Stat(2),
                false,
                true,
                false,
                false,
                new Stat(1, false)
            )
        },

    };

    public static readonly Dictionary<string, IEffect> ChainEffects = new()
    {

        {
            "Gun card effect 2",
            new AddEffectOn(
                "Gun card effect 2",
                "+50% firerate for 2 sec on reload",
                "Player",
                new PropTrigger
                {
                    Name = nameof(Player.ReloadEndEvent),
                    Path = ""
                },
                EffectRepository.StatModEffects["Secondary Gun card effect 2"]
            )
        }
    };
}