using System;
using System.Collections.Generic;

[Serializable]
public class EffectRepository : IEffectRepository
{
    private static readonly Dictionary<string, AttachModAdderEffect> AttachModAdderEffects = new()
    {
        //Gun1
        ["PlayerDmgMulti50"] = new AttachModAdderEffect
        (
            "PlayerDmgMulti50",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.Firearm) + "." + nameof(Player.Damage)),
            }
        ),
        ["PlayerFirearmMagazineCapacityAdd2"] = new AttachModAdderEffect
        (
            "PlayerFirearmMagazineCapacityAdd2",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 2f), nameof(Player.Firearm) + "." + nameof(Firearm.MagazineCapacity)),
            }
        ),
        //Gun2
        ["PlayerFirearmShootsPerSecondMulti50Temp2"] = new AttachModAdderEffect
        (
            "PlayerFirearmFirerateMulti50Temp2DurUpd",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.Firearm) + "." + nameof(Firearm.ShootsPerSecond)),
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
        ["PlayerFirearmProjectilePierceAdd2"] = new AttachModAdderEffect
        (
            "PlayerFirearmProjectilePierceAdd2",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 2f), nameof(Player.Firearm) + "." + nameof(Firearm.ProjectilePierce)),
            }
        ),
        ["PlayerCurrentFirearmShootForceMulti100"] = new AttachModAdderEffect
        (
            "PlayerCurrentFirearmShootForceMulti100",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Multiplication, 100f), nameof(Player.Firearm) + "." + nameof(Firearm.ShootForce)),
            }
        ),
        //GunTurret1
        ["PlayerTurretHubTurretCountAdd1"] = new AttachModAdderEffect
        (
            "PlayerTurretHubTurretCountAdd1",
            "",
            nameof(Player),
            new List<(StatModifier mod, string statPath)>
            {
                (new StatModifier(OperationType.Addition, 1f), nameof(Player.TurretHub) + "." + nameof(TurretHub.TurretCount)),
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
                (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." + nameof(Firearm.ShootsPerSecond)),
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
                (new StatModifier(OperationType.Addition, 2f), nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." + nameof(Firearm.SingleShootProjectile)),
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
                (new StatModifier(OperationType.Multiplication, 100f), nameof(Player.Firearm) + "." + nameof(Firearm.ShootForce)),
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
            nameof(Player.TurretHub),
            true
        ),
    };

    private static readonly Dictionary<string, EffectAdderOnEventEffect> EffectAdderOnEventEffects = new()
    {
        //Gun 2
        ["PlayerFirearmReloadEndEventPlayerFirearmShootsPerSecondMulti50Temp2"] = new EffectAdderOnEventEffect
        (
            "PlayerFirearmReloadEndEventPlayerFirearmShootsPerSecondMulti50Temp2",
            "",
            nameof(Player),
            new PropTrigger
            {
                Name = nameof(Firearm.ReloadEndEvent),
                Path = nameof(Player.Firearm)
            },
            AttachModAdderEffects["PlayerFirearmShootsPerSecondMulti50Temp2"]
        ),
    };

    private static readonly Dictionary<string, EffectAdderWhileTrueEffect> EffectAdderWhileTrueEffects = new()
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
            ToggleEffects["PlayerIsSameTurretTargetTrue"]
        ),
        //Gun Turret 3
        ["PlayerCurrentFirearmMagazineIsFullPlayerCurrentTurretFirearmSingleShootProjectileAdd2"] = new EffectAdderWhileTrueEffect
        (
            "",
            "While magazine full all turrets get +2 projectiles",
            nameof(Player),
            new PropTrigger
            {
                Name = nameof(Firearm.Magazine.FillEvent),
                Path = nameof(Player.Firearm) + "." + nameof(Firearm.Magazine)
            },
            new PropTrigger
            {
                Name = nameof(Firearm.Magazine.DecreaseEvent),
                Path = nameof(Player.Firearm) + "." + nameof(Firearm.Magazine)
            },
            nameof(Player.Firearm) + "." +
            nameof(Firearm.Magazine) + "." +
            nameof(Resource.IsFull),
            AttachModAdderEffects["PlayerCurrentTurretFirearmSingleShootProjectileAdd2"]
        ),
    };

    private static readonly Dictionary<string, AttachEffectAdderEffect> CardEffects = new()
    {
        //Gun
        [nameof(CardTag.Gun) + 1] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + 1,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>()
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
                (effect: EffectAdderOnEventEffects["PlayerFirearmReloadEndEventPlayerFirearmShootsPerSecondMulti50Temp2"], stackCount: 1)
            }
        ),
        [nameof(CardTag.Gun) + 3] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + 3,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: AttachModAdderEffects["PlayerFirearmProjectilePierceAdd2"], stackCount: 1),
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
                (effect: AttachModAdderEffects["PlayerTurretHubTurretCountAdd1"], stackCount: 1),
            }
        ),
        [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2] = new AttachEffectAdderEffect
        (
            nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2,
            "",
            nameof(Player),
            new List<(IEffect effect, int stackCount)>
            {
                (effect: EffectAdderWhileTrueEffects["PlayerFireEventFireOffEventIsFireButtonPressedPlayerIsSameTurretTargetTrue"], stackCount: 1),
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

    public IEffect Get(string effectName)
    {
        return CardEffects[effectName];
    }

    private static IEffect GetModAdder(string effectName)
    {
        return AttachModAdderEffects[effectName];
    }
    
    private static IEffect GetToggle(string effectName)
    {
        return ToggleEffects[effectName];
    }
    
    private static IEffect GetEffectAdder(string effectName)
    {
        return EffectAdderOnEventEffects[effectName];
    }
    
    private static IEffect GetEffectAdderWhile(string effectName)
    {
        return EffectAdderWhileTrueEffects[effectName];
    }

    //public EffectRepository() : this(CardEffects)
    //{
    //}

    //public EffectRepository(Dictionary<string, AttachEffectAdderEffect> effects)
    //{
    //    _effects = effects;
    //}
}