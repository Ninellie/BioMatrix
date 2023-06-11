using System;
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
        private static readonly Dictionary<string, IncreaserResourceOnEventEffect> IncreaseResourceOnEffects = new()
        {
            // Vitality Experience 2
            [nameof(Player) + "Increase" + nameof(Player.LifePoints) + 1 + nameof(Player.ExperienceTakenEvent) + 20] = new IncreaserResourceOnEventEffect
            (
                nameof(Player) + "Increase" + nameof(Player.LifePoints) + 1 + nameof(Player.ExperienceTakenEvent) + 20,
                "",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Player.ExperienceTakenEvent),
                    Path = ""
                },
                new List<(int value, string resourcePath)>
                {
                    (1, nameof(Player.LifePoints)),
                },
                20,
                false
            ),
        };
        
        private static readonly Dictionary<string, AttachResourceIncreaserEffect> AttachResourceAdderEffects = new()
        {
            // Turret
            ["PlayerTurretHubTurretsAdd1"] = new AttachResourceIncreaserEffect
            (
                "PlayerTurretHubTurretsAdd1",
                "",
                nameof(Player),
                new List<(int value, string resourcePath)>
                {
                    (1, nameof(Player.TurretHub) + "." + nameof(TurretHub.Turrets)),
                },
                true
            ),
        };

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
                    (new StatModifier(OperationType.Multiplication, 50f),
                        nameof(Player.Firearm) + "." + nameof(Player.Damage)),
                }
            ),
            ["PlayerFirearmMagazineCapacityAdd2"] = new AttachModAdderEffect
            (
                "PlayerFirearmMagazineCapacityAdd2",
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Addition, 2f),
                        nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.MagazineCapacity)),
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
                    (new StatModifier(OperationType.Multiplication, 50f),
                        nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
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
                    (new StatModifier(OperationType.Addition, 2f),
                        nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ProjectilePierceCount)),
                }
            ),
            ["PlayerFirearmShootForceMulti50"] = new AttachModAdderEffect
            (
                "PlayerFirearmShootForceMulti50",
                "",
                nameof(Player),
                new List<(StatModifier mod, string statPath)>
                {
                    (new StatModifier(OperationType.Multiplication, 50),
                        nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootForce)),
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
                    (new StatModifier(OperationType.Multiplication, 50),
                        nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                        nameof(Firearm.Firearm.ShootsPerSecond)),
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
                    (new StatModifier(OperationType.Addition, 2f),
                        nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                        nameof(Firearm.Firearm.SingleShootProjectile)),
                }
            ),
            //Gun Vitality 1
            [nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 1f), nameof(Player.MaximumLifePoints)),
                    },
                    true
                ),
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ProjectileSizeMultiplier) + nameof(OperationType.Addition) + 100] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ProjectileSizeMultiplier) +
                    nameof(OperationType.Addition) + 100,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 100f),
                            nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ProjectileSizeMultiplier)),
                    }
                ),
            //Gun Vitality 2
            [nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60"] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.LifeRegenerationPerSecond) + nameof(OperationType.Addition) + "1/60",
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 1 / 60f), nameof(Player.LifeRegenerationPerSecond)),
                    }
                ),
            //Gun Vitality 3
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 100] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) +
                    nameof(OperationType.Multiplication) + 100,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 100f),
                            nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
                    }
                ),
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.Damage) + nameof(OperationType.Multiplication) + 50] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.Damage) +
                    nameof(OperationType.Multiplication) + 50,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50f),
                            nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.Damage)),
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
                    (new StatModifier(OperationType.Multiplication, 5),
                        nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
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
            [nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 25), nameof(Player.Speed)),
                    },
                    true
                ),
            [nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 25] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) +
                    nameof(OperationType.Multiplication) + 25,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 25),
                            nameof(Player.Firearm) + "." + nameof(Firearm.Firearm.ShootsPerSecond)),
                    }
                ),
            // Gun Movement Experience 3
            [nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 25), nameof(Player.ExpMultiplier)),
                    },
                    true
                ),
            //Turret 1

            //Turret 2
            [nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.SingleShootProjectile) +
                    nameof(OperationType.Addition) + 2 + "Temp" + 2,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 2f),
                            nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                            nameof(Firearm.Firearm.SingleShootProjectile)),
                    },
                    true,
                    new Stat.Stat(2, true),
                    false,
                    true,
                    false,
                    false,
                    new Stat.Stat(1, false)
                ),
            //Turret 3
            [nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.ProjectilePierceCount) + nameof(OperationType.Addition) + 1] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.ProjectilePierceCount) +
                    nameof(OperationType.Addition) + 1,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 1),
                            nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                            nameof(Firearm.Firearm.ProjectilePierceCount)),
                    }
                ),
            [nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 50] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.ShootsPerSecond) +
                    nameof(OperationType.Multiplication) + 50,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50),
                            nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                            nameof(Firearm.Firearm.ShootsPerSecond)),
                    }
                ),
            // Turret Shield 1
            [nameof(Player) + nameof(Player.MaxRechargeableShieldLayersCount) + nameof(OperationType.Addition) + 1] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.MaxRechargeableShieldLayersCount) + nameof(OperationType.Addition) +
                    1,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Addition, 1), nameof(Player.MaxRechargeableShieldLayersCount)),
                    }
                ),
            // Turret Shield 2
            [nameof(Player) + nameof(Player.ShieldLayerRechargeRatePerSecond) + nameof(OperationType.Multiplication) + 100] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.ShieldLayerRechargeRatePerSecond) +
                    nameof(OperationType.Multiplication) + 100,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 100),
                            nameof(Player.ShieldLayerRechargeRatePerSecond)),
                    }
                ),
            // Turret Shield 3
            [nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.Damage) + nameof(OperationType.Multiplication) + 50] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.Damage) +
                    nameof(OperationType.Multiplication) + 50,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 50),
                            nameof(Player.TurretHub) + "." + nameof(TurretHub.Firearm) + "." +
                            nameof(Firearm.Firearm.Damage)),
                    }
                ),
            // Turret Movement Magnetism 1
            [nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 25), nameof(Player.MagnetismRadius)),
                    },
                    true
                ),
            // Vitality Experience 1

            // Vitality Experience 3
            [nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100] =
                new AttachModAdderEffect
                (
                    nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100,
                    "",
                    nameof(Player),
                    new List<(StatModifier mod, string statPath)>
                    {
                        (new StatModifier(OperationType.Multiplication, 100), nameof(Player.ExpMultiplier)),
                    },
                    false
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
            // Gun 2
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
            // Gun Movement Experience 1
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
            // Turret 2
            [nameof(Player) + nameof(TurretHub.KillsCount.IncrementEvent) + nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2] =
                new EffectAdderOnEventEffect
                (
                    nameof(Player) + nameof(TurretHub.KillsCount.IncrementEvent) + nameof(Player) + nameof(TurretHub) +
                    nameof(Firearm.Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2,
                    "",
                    nameof(Player),
                    new PropTrigger
                    {
                        Name = nameof(TurretHub.KillsCount.IncrementEvent),
                        Path = nameof(Player.TurretHub) + "." + nameof(TurretHub.KillsCount)
                    },
                    AttachModAdderEffects[
                        nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.SingleShootProjectile) +
                        nameof(OperationType.Addition) + 2 + "Temp" + 2]
                ),
        };

        private static readonly Dictionary<string, EffectAdderWhileTrueEffect> EffectAdderWhileTrueEffects = new()
        {
            // Gun Turret 2
            ["GunTurret2_While"] = new EffectAdderWhileTrueEffect
            (
                "GunTurret2_While",
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
            // Gun Turret 3
            ["GunTurret3_While"] = new EffectAdderWhileTrueEffect
            (
                "GunTurret3_While",
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
            // Gun Vitality 3
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
                AttachModAdderEffects[
                    nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.ShootsPerSecond) +
                    nameof(OperationType.Multiplication) + 100]
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
                AttachModAdderEffects[
                    nameof(Player) + nameof(Firearm.Firearm) + nameof(Firearm.Firearm.Damage) +
                    nameof(OperationType.Multiplication) + 50]
            ),
            // Turret Shield 3
            ["TurretShield3_While_1"] = new EffectAdderWhileTrueEffect
            (
                "TurretShield3_While_1",
                "+ 1 turret while has no shield",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Resource.EmptyEvent),
                    Path = nameof(Player.ShieldLayers)
                },
                new PropTrigger
                {
                    Name = nameof(Resource.NotEmptyEvent),
                    Path = nameof(Player.ShieldLayers)
                },
                nameof(Player.ShieldLayers) + "." + nameof(Resource.IsEmpty),
                AttachResourceAdderEffects["PlayerTurretHubTurretsAdd1"]
            ),
            // Turret Shield 3
            ["TurretShield3_While_2"] = new EffectAdderWhileTrueEffect
            (
                "TurretShield3_While_2",
                "+ 50 turret damage multi while has shield",
                nameof(Player),
                new PropTrigger
                {
                    Name = nameof(Resource.NotEmptyEvent),
                    Path = nameof(Player.ShieldLayers)
                },
                new PropTrigger
                {
                    Name = nameof(Resource.EmptyEvent),
                    Path = nameof(Player.ShieldLayers)
                },
                nameof(Player.ShieldLayers) + "." + nameof(Resource.IsNotEmpty),
                AttachModAdderEffects[nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.Damage) + nameof(OperationType.Multiplication) + 50]
            ),
            // Vitality Experience 3
            ["VitalityExperience3_While_1"] = new EffectAdderWhileTrueEffect
            (
                "VitalityExperience3_While_1",
                "While life points is on edge add +100% experience gain multi",
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
                AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 100]
            ),
        };

        private static readonly Dictionary<string, EffectAdderPerResource> EffectAdderPerResourceEffects = new()
        {
            // Turret Movement Magnetism 3
            ["10msPerTurret"] = new EffectAdderPerResource
            (
                "10msPerTurret",
                "",
                nameof(Player),
                AttachModAdderEffects[nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25],
                new PropTrigger
                {
                    Name = nameof(Player.TurretHub.Turrets.ValueChangedEvent),
                    Path = nameof(Player.TurretHub) + "." + nameof(TurretHub.Turrets)
                }
            )
        };
        
        private static readonly Dictionary<string, AttachEffectAdderEffect> CardEffects = new()
        {
            // Gun
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
                    (effect: AttachResourceAdderEffects["PlayerTurretHubTurretsAdd1"], stackCount: 1),
                }
            ),
            [nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Gun) + nameof(CardTag.Turret) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderWhileTrueEffects["GunTurret2_While"], stackCount: 1),
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
                    (effect: EffectAdderWhileTrueEffects["GunTurret3_While"], stackCount: 1)
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
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25], stackCount: 4),
                }
            ),
            // Turret
            [nameof(CardTag.Turret) + 1] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + 1,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachResourceAdderEffects["PlayerTurretHubTurretsAdd1"], stackCount: 1),
                }
            ),
            [nameof(CardTag.Turret) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderOnEventEffects[nameof(Player) + nameof(TurretHub.KillsCount.IncrementEvent) + nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.SingleShootProjectile) + nameof(OperationType.Addition) + 2 + "Temp" + 2], stackCount: 1),
                }
            ),
            [nameof(CardTag.Turret) + 3] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + 3,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.ProjectilePierceCount) + nameof(OperationType.Addition) + 1], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(TurretHub) + nameof(Firearm.Firearm.ShootsPerSecond) + nameof(OperationType.Multiplication) + 50], stackCount: 1),
                }
            ),
            // Turret Shield
            [nameof(CardTag.Turret) + nameof(CardTag.Shield) + 1] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + nameof(CardTag.Shield) + 1,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MaxRechargeableShieldLayersCount) + nameof(OperationType.Addition) + 1], stackCount: 1),
                }
            ),
            [nameof(CardTag.Turret) + nameof(CardTag.Shield) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + nameof(CardTag.Shield) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.ShieldLayerRechargeRatePerSecond) + nameof(OperationType.Multiplication) + 100], stackCount: 1),
                }
            ),
            [nameof(CardTag.Turret) + nameof(CardTag.Shield) + 3] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + nameof(CardTag.Shield) + 3,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderWhileTrueEffects["TurretShield3_While_1"], stackCount: 1),
                    (effect: EffectAdderWhileTrueEffects["TurretShield3_While_2"], stackCount: 1),
                }
            ),
            // Turret Movement Magnetism
            [nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 1] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 1,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderPerResourceEffects["10msPerTurret"], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                }
            ),
            [nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.Speed) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                }
            ),
            [nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 3] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Turret) + nameof(CardTag.Movement) + nameof(CardTag.Magnetism) + 3,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachResourceAdderEffects["PlayerTurretHubTurretsAdd1"], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MagnetismRadius) + nameof(OperationType.Multiplication) + 25], stackCount: 2),
                }
            ),
            // Vitality Experience
            [nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 1] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 1,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.MaximumLifePoints) + nameof(OperationType.Addition) + 1], stackCount: 1),
                    (effect: AttachModAdderEffects[nameof(Player) + nameof(Player.ExpMultiplier) + nameof(OperationType.Multiplication) + 25], stackCount: 1),
                }
            ),
            [nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 2] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 2,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: IncreaseResourceOnEffects[nameof(Player) + "Increase" + nameof(Player.LifePoints) + 1 + nameof(Player.ExperienceTakenEvent) + 20], stackCount: 1),
                }
            ),
            [nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 3] = new AttachEffectAdderEffect
            (
                nameof(CardTag.Vitality) + nameof(CardTag.Experience) + 3,
                "",
                nameof(Player),
                new List<(IEffect effect, int stackCount)>
                {
                    (effect: EffectAdderWhileTrueEffects["VitalityExperience3_While_1"], stackCount: 1),
                }
            ),

        };

        public IEffect Get(string effectName)
        {
            return CardEffects[effectName];
        }
    }
}