using System;
using System.Collections.Generic;


[Serializable]
public static class EffectRepository
{
    public static readonly Dictionary<string, IEffect> CardEffects = new()
    {
        [nameof(CardTag.Gun) + 1] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + 1,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: AttachModAdderEffects["PlayerDmgMulti50"], stackCount: 1),
                (effect: AttachModAdderEffects["PlayerFirearmMagazineCapacityAdd2"], stackCount: 1)
            }
        ),
        [nameof(CardTag.Gun) + 2] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + 2,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: EffectAdderOnEventEffects["PlayerReloadEndEventPlayerCurrentFirearmShootsPerSecondMulti50Temp2"], stackCount: 1)
            }
        ),
        [nameof(CardTag.Gun) + 3] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + 3,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: AttachModAdderEffects["PlayerCurrentFirearmProjectilePierceAdd2"], stackCount: 1),
                (effect: AttachModAdderEffects["PlayerCurrentFirearmShootForceMulti100"], stackCount: 1)
            }
        ),
        //Gun, Turret
        [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 1] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + nameof(CardTag.Turret) + 1,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: AttachModAdderEffects["PlayerTurretCountAdd1"], stackCount: 1),
            }
        ),
        [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: ToggleEffects["PlayerFireEventFireOffEventIsFireButtonPressedPlayerIsSameTurretTargetTrue"], stackCount: 1),
                (effect: AttachModAdderEffects["PlayerCurrentTurretFirearmShootsPerSecondMulti50"], stackCount: 1)
            }
        ),
        [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 3] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + nameof(CardTag.Turret) + 3,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: EffectAdderWhileTrueEffects["PlayerCurrentFirearmMagazineIsFullPlayerCurrentTurretFirearmSingleShootProjectileAdd2"], stackCount: 1)
            }
        ),

    };
    
    private static readonly Dictionary<string, IEffect> AttachModAdderEffects = new()
    {
        //Gun1
        ["PlayerDmgMulti50"] = new AttachModAdderEffect
        (
            "PlayerDmgMulti50",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.Damage)),
            }
        ),
        ["PlayerFirearmMagazineCapacityAdd2"] = new AttachModAdderEffect
        (
            "PlayerFirearmMagazineCapacityAdd2",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 2f), nameof(Player.CurrentFirearm) + "." + nameof(Firearm.MagazineCapacity)),
            }
        ),
        //Gun2
        ["PlayerCurrentFirearmShootsPerSecondMulti50Temp2"] = new AttachModAdderEffect
        (
            "PlayerFirearmFirerateMulti50Temp2DurUpd",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.CurrentFirearm) + "." + nameof(Firearm.ShootsPerSecond)),
            },
            true,
            new Stat(2),
            false,
            true,
            false,
            false,
            new Stat(1, false)
        ),
        //Gun3
        ["PlayerCurrentFirearmProjectilePierceAdd2"] = new AttachModAdderEffect
        (
            "PlayerCurrentFirearmProjectilePierceAdd2",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 2f), nameof(Player.CurrentFirearm) + "." + nameof(Firearm.ProjectilePierce)),
            }
        ),
        ["PlayerCurrentFirearmShootForceMulti100"] = new AttachModAdderEffect
        (
            "PlayerCurrentFirearmShootForceMulti100",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 100f), nameof(Player.CurrentFirearm) + "." + nameof(Firearm.ShootForce)),
            }
        ),
        //GunTurret1
        ["PlayerTurretCountAdd1"] = new AttachModAdderEffect
        (
            "PlayerTurretCountAdd1",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 1f), nameof(Player.TurretCount)),
            }
        ),
        //Gun Turret 2
        ["PlayerCurrentTurretFirearmShootsPerSecondMulti50"] = new AttachModAdderEffect
        (
            "PlayerCurrentTurretFirearmShootsPerSecondMulti50",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.CurrentTurretFirearm) + "." + nameof(Firearm.ShootsPerSecond)),
            }
        ),
        //Gun Turret 3
        ["PlayerCurrentTurretFirearmSingleShootProjectileAdd2"] = new AttachModAdderEffect
        (
            "PlayerCurrentTurretFirearmSingleShootProjectileAdd2",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 2f), nameof(Player.CurrentTurretFirearm) + "." + nameof(Firearm.SingleShootProjectile)),
            }
        ),
        //Gun Vitality 1
        ["PlayerMaxLPAdd1"] = new AttachModAdderEffect
        (
            "",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 1f), nameof(Player.MaximumLifePoints)),
            }
        ),
        ["PlayerCurrentFirearmShootForceMulti100"] = new AttachModAdderEffect
        (
            "PlayerCurrentFirearmShootForceMulti100",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 100f), nameof(Player.CurrentFirearm) + "." + nameof(Firearm.ShootForce)),
            }
        ),
    };

    private static readonly Dictionary<string, ToggleOnAttach> ToggleEffects = new()
    {
        //Gun Turret 2
        ["PlayerIsSameTurretTargetTrue"] = new ToggleOnAttach
        (
            "PlayerIsSameTurretTargetTrue",
            "",
            nameof(Player),
            nameof(Player.IsSameTurretTarget),
            true
        ),
    };

    private static readonly Dictionary<string, EffectAdderOnEventEffect> EffectAdderOnEventEffects = new()
    {
        //Gun 2
        ["PlayerReloadEndEventPlayerCurrentFirearmShootsPerSecondMulti50Temp2"] = new EffectAdderOnEventEffect
        (
            "PlayerReloadEndEventPlayerCurrentFirearmShootsPerSecondMulti50Temp2",
            "",
            nameof(Player),
            new PropTrigger
            {
                Name = nameof(Player.ReloadEndEvent),
                Path = ""
            },
            AttachModAdderEffects["PlayerCurrentFirearmShootsPerSecondMulti50Temp2"]
        ),
    };


    public static readonly Dictionary<string, IEffect> EffectAdderWhileTrueEffects = new()
    {
        //Gun Turret 2
        ["PlayerFireEventFireOffEventIsFireButtonPressedPlayerIsSameTurretTargetTrue"] = new EffectAdderWhileTrueEffect
        (
            "PlayerFireEventFireOffEventIsFireButtonPressedPlayerIsSameTurretTargetTrue",
            "",
            nameof(Player),
            new PropTrigger
            {
                Name = nameof(Player.FireEvent),
                Path = ""
            }
            ,
            new PropTrigger
            {
                Name = nameof(Player.FireOffEvent),
                Path = ""
            },
            nameof(Player.IsFireButtonPressed),
            EffectRepository.ToggleEffects["PlayerIsSameTurretTargetTrue"]
        ),
        //Gun Turret 3
        ["PlayerCurrentFirearmMagazineIsFullPlayerCurrentTurretFirearmSingleShootProjectileAdd2"] = new EffectAdderWhileTrueEffect
        (
            "",
            "While magazine full all turrets get +2 projectiles",
            nameof(Player),
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
            nameof(Player.CurrentFirearm) + "." +
            nameof(Player.CurrentFirearm.Magazine) + "." +
            nameof(Player.CurrentFirearm.Magazine.IsFull),
            EffectRepository.AttachModAdderEffects["PlayerCurrentTurretFirearmSingleShootProjectileAdd2"]
        ),
    };
}