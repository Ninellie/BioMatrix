using System;
using System.Collections.Generic;

[Serializable]
public static class EffectRepository
{
    public static readonly Dictionary<string, IEffect> StatModEffects = new()
    {
        {
            "GunCard1",
            new AddModOnAttach
            (
                "Gun card effect 1",
                "+50% projectile damage multiplier, +2 magazine amount",
                nameof(Player),
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Multiplication, 50f), "ProjectileDamage"),
                    (new StatModifier(OperationType.Addition, 2f), "MagazineAmount"),
                }
            )
        },
        {
            "SecondaryGunCard2",
            new AddModOnAttach
            (
                "Secondary Gun card effect 2",
                "+50% firerate multiplier for 2 sec",
                nameof(Player),
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
        {
            "GunCard3",
            new AddModOnAttach
            (
                "Gun card effect 3",
                "+2 projectile pierce, +100% projectile speed",
                nameof(Player),
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Addition, 2f), "ProjectilePierce"),
                    (new StatModifier(OperationType.Multiplication, 100f), "ProjectileSpeed"),
                }
            )
        },
        {
            "GunTurretCard1",
            new AddModOnAttach
            (
                "Gun Turret card effect 1",
                "+1 turret",
                nameof(Player),
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Addition, 1f), "TurretCount"),
                }
            )
        },
        {
            "GunTurretCard2",
            new AddModOnAttach
            (
                "Gun Turret card effect 2",
                "+50% turret firerate multiplier",
                nameof(Player),
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Addition, 1f), "TurretCount"),
                }
            )
        },
        {
            "SecondaryGunTurretCard3",
            new AddModOnAttach
            (
                "+2 Turret Projectile",
                "+2 turret projectile",
                nameof(Player),
                new List<(StatModifier mod, string statName)>
                {
                    (new StatModifier(OperationType.Addition, 2f), "TurretProjectileCount"),
                }
            )
        },
    };

    public static readonly Dictionary<string, IEffect> ToggleEffects = new()
    {
        {
            "SecondaryGunTurretCard2",
            new ToggleOnAttach
            (
                "Turrets shoot where the player shoots",
                "Turrets shoot where the player shoots",
                nameof(Player),
                nameof(Player.IsSameTurretTarget),
                true
            )
        },
    };

    public static readonly Dictionary<string, IEffect> ChainEffects = new()
    {
        {
            "GunCard2",
            new AddEffectOn
            (
                "Gun card effect 2",
                "+50% firerate for 2 sec on reload",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Player.ReloadEndEvent),
                    Path = ""
                },
                EffectRepository.StatModEffects["Secondary Gun card effect 2"]
            )
        },
        {
            "SecondaryGunTurretCard2",
            new AddEffectWhileTrue
            (
                "Secondary Gun Turret card effect 2",
                "Turrets shoot where the player shoots, if fire button is pressed",
                nameof(Player),
                EffectRepository.ToggleEffects["Secondary Gun Turret card effect 2"],
                new PropTrigger
                {
                    Name = nameof(Player.FireEvent),
                    Path = ""
                }
                ,
                new PropTrigger
                {
                    Name = nameof(Player.FireEvent),
                    Path = ""
                },
                nameof(Player.IsFireButtonPressed)
            )
        },
        {
            "GunTurretCard2",
            new AddEffectOnAttach
            (
                "GunTurretCard2",
                "Turrets shoot where the player shoots, +50% turret firerate",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (EffectRepository.ChainEffects["SecondaryGunTurretCard2"], 1),
                    (EffectRepository.StatModEffects["GunTurretCard2"], 1)
                }
            )
        },
        {
            "GunTurretCard3",
            new AddEffectWhileTrue
            (
                "Gun Turret card effect 3",
                "While magazine full all turrets get +2 projectiles",
                nameof(Player),
                EffectRepository.StatModEffects["SecondaryGunTurretCard3"],
                new PropTrigger
                {
                    Name = nameof(Player.CurrentFirearm.Magazine.FillEvent),
                    Path = nameof(Player.CurrentFirearm) + "." + nameof(Player.CurrentFirearm.Magazine)
                },
                new PropTrigger
                {
                    Name = nameof(Player.CurrentFirearm.Magazine.DecreaseEvent),
                    Path = nameof(Player.CurrentFirearm) + "." + nameof(Player.CurrentFirearm.Magazine)
                },
                nameof(Player.CurrentFirearm) +
                "." +
                nameof(Player.CurrentFirearm.Magazine) +
                "." +
                nameof(Player.CurrentFirearm.Magazine.IsFull)
            )
        },
    };
}