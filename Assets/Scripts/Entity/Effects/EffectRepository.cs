﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Entity.Stat;
using Assets.Scripts.Entity.Unit.Player;
using Assets.Scripts.Entity.Unit.Turret;
using Assets.Scripts.GameSession.Upgrades.Deck;

namespace Assets.Scripts.Entity.Effects
{
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
                    (new StatModifier(OperationType.Addition, 2f), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.MagazineCapacity)),
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
                    (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
                },
                true,
                new Stat.Stat(2),
                false,
                true,
                false,
                false,
                new Stat.Stat(1, false)
            ),
            //Gun3
            ["PlayerFirearmProjectilePierceAdd2"] = new AttachModAdderEffect
            (
                "PlayerFirearmProjectilePierceAdd2",
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Addition, 2f), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ProjectilePierceCount)),
                }
            ),
            ["PlayerFirearmShootForceMulti50"] = new AttachModAdderEffect
            (
                "PlayerFirearmShootForceMulti50",
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 50), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootForce)),
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
                    (new StatModifier(OperationType.Addition, 1), nameof(Player.TurretHub) + "." + nameof(TurretHub.TurretCount)),
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
                    (new StatModifier(OperationType.Multiplication, 50), nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
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
                    (new StatModifier(OperationType.Addition, 2f), nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." + nameof(Firearm.Firearm.SingleShootProjectile)),
                }
            ),
            //Gun Vitality 1
            [nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Addition, 1f), nameof(Player.MaximumLifePoints)),
                }
            ),
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ProjectileSizeMultiplier) + nameof(OperationType.Addition) + 100] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ProjectileSizeMultiplier) + nameof(OperationType.Addition) + 100,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Addition, 100f), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ProjectileSizeMultiplier)),
                }
            ),
            //Gun Vitality 2
            [nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60"] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60",
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Addition, 1/60f), nameof(Player.LifeRegenerationPerSecond)),
                }
            ),
            //Gun Vitality 3
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 100] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 100,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 100f), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
                }
            ),
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.Damage) + nameof(OperationType.Multiplication) + 50] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.Damage) + nameof(OperationType.Multiplication) + 50,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 50f), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.Damage)),
                }
            ),
            // Gun Movement Experience 1
            ["Adrenalin"] = new AttachModAdderEffect
            (
                "Adrenalin",
                "Adrenalin",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 5), nameof(Player.Speed)),
                    (new StatModifier(OperationType.Multiplication, 5), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
                },
                true,
                new Stat.Stat(5, true),
                false,
                true,
                true,
                false,
                new Stat.Stat(50, true)
            ),
            // Gun Movement Experience 2
            [nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 25), nameof(Player.Speed)),
                }
            ),
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 25] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 25,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 25), nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
                }
            ),
            // Gun Movement Experience 3
            [nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100] = new AttachModAdderEffect
            (
                nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100,
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 100), nameof(Player.ExpMultiplier)),
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
                nameof(Player.TurretHub) + "." + nameof(TurretHub.IsSameTurretTarget),
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
                    Name = nameof(Firearm.Firearm.ReloadEndEvent),
                    Path = nameof(Player.Firearm)
                },
                AttachModAdderEffects["PlayerFirearmShootsPerSecondMulti50Temp2"]
            ),
            //Gun Movement Experience 1
            [nameof(Player) + nameof(Player.ExperienceTakenEvent) + "Adrenalin"] = new EffectAdderOnEventEffect
            (
                nameof(Player) + nameof(Player.ExperienceTakenEvent) + "Adrenalin",
                "Adrenalin's source",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Player.ExperienceTakenEvent),
                    Path = ""
                },
                AttachModAdderEffects["Adrenalin"]
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
                "PlayerCurrentFirearmMagazineIsFullPlayerCurrentTurretFirearmSingleShootProjectileAdd2",
                "While magazine full all turrets get +2 projectiles",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Firearm.Firearm.Magazine.FillEvent),
                    Path = nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.Magazine)
                },
                new PropTrigger
                {
                    Name = nameof(Firearm.Firearm.Magazine.DecreaseEvent),
                    Path = nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.Magazine)
                },
                nameof(Player.Firearm) + "." +
                nameof(Firearm.Firearm.Magazine) + "." +
                nameof(Resource.IsFull),
                AttachModAdderEffects["PlayerCurrentTurretFirearmSingleShootProjectileAdd2"]
            ),
            //Gun Vitality 3
            ["GunVitality3_While_1"] = new EffectAdderWhileTrueEffect
            (
                "GunVitality3_While_1",
                "While life points is on edge add +100% firerate multi",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Resource.EdgeEvent),
                    Path = nameof(Player.LifePoints)
                },
                new PropTrigger
                {
                    Name = nameof(Resource.NotEdgeEvent),
                    Path = nameof(Player.LifePoints)
                },
                nameof(Player.LifePoints) + "." + nameof(Resource.IsOnEdge),
                AttachModAdderEffects[nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 100]
            ),
            ["GunVitality3_While_2"] = new EffectAdderWhileTrueEffect
            (
                "GunVitality3_While_2",
                "While life points is on edge add +50% damage multi",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Resource.EdgeEvent),
                    Path = nameof(Player.LifePoints)
                },
                new PropTrigger
                {
                    Name = nameof(Resource.NotEdgeEvent),
                    Path = nameof(Player.LifePoints)
                },
                nameof(Player.LifePoints) + "." + nameof(Resource.IsOnEdge),
                AttachModAdderEffects[nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.Damage) + nameof(OperationType.Multiplication) + 50]
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
                    (effect: AttachModAdderEffects["PlayerFirearmShootForceMulti50"], stackCount: 1)
                }
            ),
            // Gun Turret
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
            // Gun Vitality
            [nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 1] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 1,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ProjectileSizeMultiplier) + nameof(OperationType.Addition) + 100], stackCount: 1),
                }
            ),
            [nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60"], stackCount: 1)
                }
            ),
            [nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 3] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Vitality) + 3,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderWhileTrueEffects["GunVitality3_While_1"], stackCount: 1),
                    (effect: EffectAdderWhileTrueEffects["GunVitality3_While_2"], stackCount: 1)
                }
            ),
            // Gun Movement Experience
            [nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 1] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 1,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderOnEventEffects[nameof(Player) + nameof(Player.ExperienceTakenEvent) + "Adrenalin"], stackCount: 1),
                }
            ),
            [nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                }
            ),
            [nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 3] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Movement) + nameof(CardTag.Experience) + 3,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100], stackCount: 1),
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
    }
}